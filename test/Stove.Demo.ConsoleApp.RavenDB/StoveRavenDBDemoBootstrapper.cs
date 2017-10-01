using Stove.Bootstrapping;
using Stove.RavenDB;

namespace Stove.Demo.ConsoleApp.RavenDB
{
    [DependsOn(
        typeof(StoveRavenDBBootstrapper)
    )]
    public class StoveRavenDBDemoBootstrapper : StoveBootstrapper
    {
    }
}
