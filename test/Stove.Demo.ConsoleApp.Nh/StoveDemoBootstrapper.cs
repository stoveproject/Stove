using Stove.Bootstrapping;

namespace Stove.Demo.ConsoleApp.Nh
{
    [DependsOn(
        typeof(StoveNHibernateBootstrapper),
        typeof(StoveDapperBootstrapper)
    )]
    public class StoveDemoBootstrapper : StoveBootstrapper
    {
    }
}
