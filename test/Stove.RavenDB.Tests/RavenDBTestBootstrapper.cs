using Stove.Bootstrapping;

namespace Stove.RavenDB.Tests
{
    [DependsOn(
        typeof(StoveRavenDBBootstrapper)
        )]
    public class RavenDBTestBootstrapper : StoveBootstrapper
    {
    }
}
