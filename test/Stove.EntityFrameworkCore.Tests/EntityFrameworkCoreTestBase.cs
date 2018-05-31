using System;
using System.Threading.Tasks;
using Autofac.Extras.IocManager;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Stove.Dapper;
using Stove.Domain.Repositories;
using Stove.EntityFrameworkCore.Tests.Domain;
using Stove.EntityFrameworkCore.Tests.Ef;
using Stove.Reflection.Extensions;
using Stove.TestBase;
using Stove.Timing;

namespace Stove.EntityFrameworkCore.Tests
{
    public abstract class EntityFrameworkCoreTestBase : ApplicationTestBase<EntityFrameworkCoreTestBootstrapper>
    {
        protected EntityFrameworkCoreTestBase() : base(true)
        {
            Clock.Provider = ClockProviders.Utc;

            Building(builder =>
            {
                builder
                .UseStoveEntityFrameworkCore()
                .UseStoveEventBus()
                .UseStoveDapper();

                RegisterInMemoryDbContext<BloggingDbContext>(builder, opt => new BloggingDbContext(opt));
                RegisterInMemoryDbContext<SupportDbContext>(builder, opt => new SupportDbContext(opt));

                builder.RegisterServices(r =>
                {
                    r.Register<IRepository<Post, Guid>, IPostRepository, PostRepository>();
                    r.RegisterAssemblyByConvention(typeof(EntityFrameworkCoreTestBase).GetAssembly());
                });
            });
        }

        protected void RegisterInMemoryDbContext<TDbContext>(IIocBuilder builder, Func<DbContextOptions<TDbContext>, TDbContext> dbContextFactory) where TDbContext : StoveDbContext
        {
            var optionsBuilder = new DbContextOptionsBuilder<TDbContext>();
            optionsBuilder.UseLazyLoadingProxies();

            optionsBuilder.ReplaceService<IEntityMaterializerSource, StoveEntityMaterializerSource>();

            var inMemorySqlLiteConnection = new SqliteConnection("Data Source=:memory:");
            optionsBuilder.UseSqlite(inMemorySqlLiteConnection);

            builder.RegisterServices(r => r.Register(ctx => optionsBuilder.Options));

            inMemorySqlLiteConnection.Open();

            dbContextFactory(optionsBuilder.Options).Database.EnsureCreated();
        }

        protected override void PostBuild()
        {
            CreateInitialData();
        }

        private void CreateInitialData()
        {
            UsingDbContext<BloggingDbContext>(
                context =>
                {
                    var blog1 = new Blog("test-blog-1", "http://testblog1.myblogs.com");

                    context.Blogs.Add(blog1);

                    var post1 = new Post { Blog = blog1, Title = "test-post-1-title", Body = "test-post-1-body" };
                    var post2 = new Post { Blog = blog1, Title = "test-post-2-title", Body = "test-post-2-body" };

                    context.Posts.AddRange(post1, post2);
                });

            UsingDbContext<SupportDbContext>(
                context =>
                {
                    context.Tickets.AddRange(
                    new Ticket { EmailAddress = "john@stove.com", Message = "an active message" },
                    new Ticket { EmailAddress = "david@stove.com", Message = "an inactive message", IsActive = false }
                    );
                });
        }

        public void UsingDbContext<TDbContext>(Action<TDbContext> action) where TDbContext : StoveDbContext
        {
            using (var context = The<TDbContext>())
            {
                action(context);
                context.SaveChanges();
            }
        }

        public T UsingDbContext<TDbContext, T>(Func<TDbContext, T> func) where TDbContext : StoveDbContext
        {
            T result;

            using (var context = The<TDbContext>())
            {
                result = func(context);
                context.SaveChanges();
            }

            return result;
        }

        public async Task UsingDbContextAsync<TDbContext>(Func<TDbContext, Task> action) where TDbContext : StoveDbContext
        {
            using (var context = The<TDbContext>())
            {
                await action(context);
                await context.SaveChangesAsync(true);
            }
        }

        public async Task<T> UsingDbContextAsync<TDbContext, T>(Func<TDbContext, Task<T>> func) where TDbContext : StoveDbContext
        {
            T result;

            using (var context = The<TDbContext>())
            {
                result = await func(context);
                context.SaveChanges();
            }

            return result;
        }
    }
}
