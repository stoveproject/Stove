using Stove.Bootstrapping;
using Stove.EntityFramework;

namespace Stove.Dapper
{
    [DependsOn(
        typeof(StoveEntityFrameworkBootstrapper),
        typeof(StoveKernelBootstrapper)
    )]
    public class StoveDapperBootstrapper : StoveBootstrapper
    {
    }
}
