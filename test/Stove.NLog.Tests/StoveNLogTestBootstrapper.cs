using Stove.Bootstrapping;

namespace Stove.NLog.Tests
{
    [DependsOn(
        typeof(StoveNLogBootstrapper)
    )]
    public class StoveNLogTestBootstrapper : StoveBootstrapper
    {
    }
}
