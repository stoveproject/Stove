using System;
using System.Reflection;
using System.Threading.Tasks;

using Autofac.Extras.IocManager;

using Effort;

using EntityFramework.DynamicFilters;

using Stove.EntityFramework;
using Stove.Runtime.Session;
using Stove.TestBase;
using Stove.Tests.SampleApplication.Domain;

namespace Stove.Tests.SampleApplication
{
    public abstract class TestBase : TestBaseWithLocalIocResolver
    {
        protected TestBase()
        {
            Building(builder =>
            {
                builder
                    .UseStove()
                    .UseStoveEntityFramework()
                    .UseDefaultEventBus()
                    .UseDefaultConnectionStringResolver()
                    .UseDbContextEfTransactionStrategy();

                builder.RegisterServices(r => r.Register(context => DbConnectionFactory.CreateTransient(), Lifetime.Singleton));
                builder.RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly()));
                builder.RegisterServices(r => r.Register<IStoveSession, TestStoveSession>());
            });
        }

        protected TestStoveSession TestStoveSession => LocalResolver.Resolve<TestStoveSession>();

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
