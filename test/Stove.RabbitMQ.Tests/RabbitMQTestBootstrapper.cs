using Stove.Bootstrapping;

namespace Stove.RabbitMQ.Tests
{
    [DependsOn(
        typeof(StoveRabbitMQBootstrapper)
        )]
    public class RabbitMQTestBootstrapper : StoveBootstrapper
    {
    }
}
