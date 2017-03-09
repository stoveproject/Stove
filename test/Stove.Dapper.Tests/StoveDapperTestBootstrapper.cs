using Stove.Bootstrapping;

namespace Stove.Dapper.Tests
{
    [DependsOn(
        typeof(StoveDapperBootstrapper)
        )]
    public class StoveDapperTestBootstrapper : StoveBootstrapper
    {
    }
}
