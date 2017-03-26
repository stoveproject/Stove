using Stove.Bootstrapping;

namespace Stove
{
    [DependsOn(
        typeof(StoveKernelBootstrapper)
    )]
    public class StoveDapperBootstrapper : StoveBootstrapper
    {
    }
}
