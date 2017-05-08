using Stove.Bootstrapping;

namespace Stove.Demo.ConsoleApp.RavenDB
{
    [DependsOn(
        typeof(StoveRavenDBBootstrapper)
    )]
    public class StoveRavenDBDemoBootstrapper : StoveBootstrapper
    {
    }
}
