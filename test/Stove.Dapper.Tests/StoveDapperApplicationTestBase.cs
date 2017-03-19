using System.Reflection;

using Autofac.Extras.IocManager;

using Effort;

using Stove.TestBase;

namespace Stove.Dapper.Tests
{
    public abstract class StoveDapperApplicationTestBase : ApplicationTestBase<StoveDapperTestBootstrapper>
    {
        protected StoveDapperApplicationTestBase()
        {
            Building(builder =>
            {
                builder
                    .UseStoveEntityFramework()
                    .UseStoveDapper()
                    .UseStoveEventBus()
                    .UseStoveDefaultConnectionStringResolver()
                    .UseStoveDbContextEfTransactionStrategy();

                builder.RegisterServices(r => r.Register(context => DbConnectionFactory.CreateTransient(), Lifetime.Singleton));
                builder.RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly()));
            });
        }
    }
}
