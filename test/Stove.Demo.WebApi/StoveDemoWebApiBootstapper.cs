using Stove.Bootstrapping;

namespace Stove.Demo.WebApi
{
    [DependsOn(
        typeof(StoveEntityFrameworkBootstrapper),
        typeof(StoveDapperBootstrapper)
    )]
    public class StoveDemoWebApiBootstapper : StoveBootstrapper
    {
        public override void PreStart()
        {
            Configuration.DefaultNameOrConnectionString = "Default";
        }
    }
}
