using System.Reflection;

using Autofac.Extras.IocManager;

using Stove.Bootstrapping;
using Stove.Runtime.Session;

namespace Stove.TestBase
{
    public abstract class ApplicationTestBase<TStarterBootstrapper> : TestBaseWithLocalIocResolver
        where TStarterBootstrapper : StoveBootstrapper
    {
        protected ApplicationTestBase()
        {
            Building(builder =>
            {
                builder
                    .UseStoveWithNullables(typeof(TStarterBootstrapper));

                builder.RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly()));
                builder.RegisterServices(r => r.Register<IStoveSession, TestStoveSession>(Lifetime.Singleton));
            });
        }

        protected TestStoveSession TestStoveSession => LocalResolver.Resolve<TestStoveSession>();

        protected override void PreBuild()
        {
        }

        protected override void PostBuild()
        {
        }
    }
}
