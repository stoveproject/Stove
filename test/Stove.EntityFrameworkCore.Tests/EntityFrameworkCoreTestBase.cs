using System;
using System.Threading.Tasks;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
				.UseStoveEventBus();

				var bloggingDbContextBuilder = new DbContextOptionsBuilder<BloggingDbContext>();
			    bloggingDbContextBuilder.UseLazyLoadingProxies();

				bloggingDbContextBuilder.ReplaceService<IEntityMaterializerSource, StoveEntityMaterializerSource>();

				var bloggingDbContextInMemorySqlite = new SqliteConnection("Data Source=:memory:");
				bloggingDbContextBuilder.UseSqlite(bloggingDbContextInMemorySqlite);

				builder.RegisterServices(r => r.Register(ctx => bloggingDbContextBuilder.Options));

				bloggingDbContextInMemorySqlite.Open();
				new BloggingDbContext(bloggingDbContextBuilder.Options).Database.EnsureCreated();




				var supportDbContextBuilder = new DbContextOptionsBuilder<SupportDbContext>();
			    supportDbContextBuilder.UseLazyLoadingProxies();

				supportDbContextBuilder.ReplaceService<IEntityMaterializerSource, StoveEntityMaterializerSource>();

				var supportDbContextInMemorySqlite = new SqliteConnection("Data Source=:memory:");
				supportDbContextBuilder.UseSqlite(supportDbContextInMemorySqlite);

				builder.RegisterServices(r => r.Register(ctx => supportDbContextBuilder.Options));

				supportDbContextInMemorySqlite.Open();
				new SupportDbContext(supportDbContextBuilder.Options).Database.EnsureCreated();

				builder.RegisterServices(r =>
				{
					r.Register<IRepository<Post, Guid>, IPostRepository, PostRepository>();
					r.RegisterAssemblyByConvention(typeof(EntityFrameworkCoreTestBase).GetAssembly());
				});

			});
		}

		protected override void PostBuild()
		{
			CreateInitialData();
		}

		private void CreateInitialData()
		{
			UsingDbContext(
				context =>
				{
					var blog1 = new Blog("test-blog-1", "http://testblog1.myblogs.com");

					context.Blogs.Add(blog1);

					var post1 = new Post { Blog = blog1, Title = "test-post-1-title", Body = "test-post-1-body" };
					var post2 = new Post { Blog = blog1, Title = "test-post-2-title", Body = "test-post-2-body" };

					context.Posts.AddRange(post1, post2);
				});

			using (var context = The<SupportDbContext>())
			{
				context.Tickets.AddRange(
					new Ticket { EmailAddress = "john@stove.com", Message = "an active message" },
					new Ticket { EmailAddress = "david@stove.com", Message = "an inactive message", IsActive = false }
				);

				context.SaveChanges();
			}
		}

		public void UsingDbContext(Action<BloggingDbContext> action)
		{
			using (var context = The<BloggingDbContext>())
			{
				action(context);
				context.SaveChanges();
			}
		}

		public T UsingDbContext<T>(Func<BloggingDbContext, T> func)
		{
			T result;

			using (var context = The<BloggingDbContext>())
			{
				result = func(context);
				context.SaveChanges();
			}

			return result;
		}

		public async Task UsingDbContextAsync(Func<BloggingDbContext, Task> action)
		{
			using (var context = The<BloggingDbContext>())
			{
				await action(context);
				await context.SaveChangesAsync(true);
			}
		}

		public async Task<T> UsingDbContextAsync<T>(Func<BloggingDbContext, Task<T>> func)
		{
			T result;

			using (var context = The<BloggingDbContext>())
			{
				result = await func(context);
				context.SaveChanges();
			}

			return result;
		}
	}
}
