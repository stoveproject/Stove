using Stove.Bootstrapping;

namespace Stove
{
    [DependsOn(
        typeof(StoveEntityFrameworkBootstrapper),
        typeof(StoveKernelBootstrapper)
    )]
    public class StoveDapperBootstrapper : StoveBootstrapper
    {
    }
}
