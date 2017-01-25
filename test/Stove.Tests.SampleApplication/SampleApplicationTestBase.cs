using System;
using System.Reflection;
using System.Threading.Tasks;

using Autofac.Extras.IocManager;

using Effort;

using EntityFramework.DynamicFilters;

using Stove.Bootstrapping;
using Stove.EntityFramework;
using Stove.Mapster;
using Stove.Runtime.Session;
using Stove.TestBase;
using Stove.Tests.SampleApplication.Domain;
using Stove.Tests.SampleApplication.Domain.Entities;
using Stove.Timing;

namespace Stove.Tests.SampleApplication
{
    public abstract class SampleApplicationTestBase<TStarterBootstrapper> : TestBaseWithLocalIocResolver
        where TStarterBootstrapper : StoveBootstrapper
    {
        protected SampleApplicationTestBase()
        {
            Building(builder =>
            {
                builder
                    .UseStove<TStarterBootstrapper>()
                    .UseStoveEntityFramework()
                    .UseStoveDefaultEventBus()
                    .UseStoveDefaultConnectionStringResolver()
                    .UseStoveDbContextEfTransactionStrategy()
                    .UseStoveMapster()
                    .UseStoveNullLogger();

                builder.RegisterServices(r => r.Register(context => DbConnectionFactory.CreateTransient(), Lifetime.Singleton));
                builder.RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly()));
                builder.RegisterServices(r => r.Register<IStoveSession, TestStoveSession>());
            });
        }

        protected TestStoveSession TestStoveSession => LocalResolver.Resolve<TestStoveSession>();

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
