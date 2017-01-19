using Stove.Bootstrapping;

namespace Stove.NLog
{
    [DependsOn(
        typeof(StoveKernelBootstrapper)
    )]
    public class StoveNLogBootstrapper : StoveBootstrapper
    {
    }
}
