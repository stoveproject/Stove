using MassTransit;

using Stove.Bootstrapping;
using Stove.RabbitMQ;

namespace Stove
{
    [DependsOn(
        typeof(StoveKernelBootstrapper)
    )]
    public class StoveRabbitMQBootstrapper : StoveBootstrapper
    {
        public override void PreStart()
        {
            Configuration.GetConfigurerIfExists<IStoveRabbitMQConfiguration>().Invoke(Configuration.Modules.StoveRabbitMQ());
        }

        public override void PostStart()
        {
            Configuration.Resolver.Resolve<IBusControl>().Start();
        }

        public override void Shutdown()
        {
            Configuration.Resolver.Resolve<IBusControl>().Stop();
        }
    }
}
