using Stove.Bootstrapping;
using Stove.Dapper;
using Stove.EntityFramework;

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
            StoveConfiguration.DefaultNameOrConnectionString = "Default";
        }
    }
}
