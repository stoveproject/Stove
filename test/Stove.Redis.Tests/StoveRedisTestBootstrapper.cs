using Stove.Bootstrapping;

namespace Stove.Redis.Tests
{
    [DependsOn(
        typeof(StoveRedisBootstrapper)
    )]
    public class StoveRedisTestBootstrapper : StoveBootstrapper
    {
    }
}
