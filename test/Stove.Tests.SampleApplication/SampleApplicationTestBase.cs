using System;
using System.Reflection;
using System.Threading.Tasks;

using Autofac.Extras.IocManager;

using Effort;

using EntityFramework.DynamicFilters;

using Stove.EntityFramework;
using Stove.Mapster;
using Stove.Runtime.Session;
using Stove.TestBase;
using Stove.Tests.SampleApplication.Domain;
using Stove.Tests.SampleApplication.Domain.Entities;
using Stove.Timing;

namespace Stove.Tests.SampleApplication
{
    public class SampleApplicationTestBase : ApplicationTestBase<SampleApplicationBootstrapper>
    {
        public SampleApplicationTestBase()
        {
            Building(builder =>
            {
                builder
                    .UseStoveEntityFramework()
                    .UseStoveEventBus()
                    .UseStoveDefaultConnectionStringResolver()
                    .UseStoveDbContextEfTransactionStrategy()
                    .UseStoveMapster();

                builder.RegisterServices(r => r.Register(context => DbConnectionFactory.CreateTransient(), Lifetime.Singleton));
                builder.RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly()));
            });
        }

        protected override void PostBuild()
        {
            CreateInitialData();
        }

        protected virtual void CreateInitialData()
        {
            UsingDbContext(context =>
            {
                context.Users.Add(new User
                {
                    CreationTime = Clock.Now,
                    Name = "Oğuzhan",
                    Surname = "Soykan",
                    Email = "oguzhansoykan@outlook.com"
                });

                context.Users.Add(new User
                {
                    CreationTime = Clock.Now,
                    Name = "Neşet",
                    Surname = "Ertaş",
                    Email = "nesetertas@hotmail.com"
                });

                context.Users.Add(new User
                {
                    CreationTime = Clock.Now,
                    Name = "Muharrem",
                    Surname = "Ertaş",
                    Email = "muharremertas@gmail.com"
                });

                context.Users.Add(new User
                {
                    CreationTime = Clock.Now,
                    Name = "Çekiç",
                    Surname = "Ali",
                    Email = "cekicali@hotmail.com"
                });
            });
        }

        public void UsingDbContext(Action<SampleApplicationDbContext> action)
        {
            using (var context = LocalResolver.Resolve<SampleApplicationDbContext>())
            {
                context.DisableAllFilters();
                action(context);
                context.SaveChanges();
            }
        }

        public T UsingDbContext<T>(Func<SampleApplicationDbContext, T> func)
        {
            T result;

            using (var context = LocalResolver.Resolve<SampleApplicationDbContext>())
            {
                context.DisableAllFilters();
                result = func(context);
                context.SaveChanges();
            }

            return result;
        }

        public async Task<T> UsingDbContextAsync<T>(Func<SampleApplicationDbContext, Task<T>> func)
        {
            T result;

            using (var context = LocalResolver.Resolve<SampleApplicationDbContext>())
            {
                context.DisableAllFilters();
                result = await func(context);
                context.SaveChanges();
            }

            return result;
        }
    }
}
