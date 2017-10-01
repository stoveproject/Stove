using Stove.Bootstrapping;

namespace Stove.Dapper
{
    [DependsOn(
        typeof(StoveKernelBootstrapper)
    )]
    public class StoveDapperBootstrapper : StoveBootstrapper
    {
    }
}
