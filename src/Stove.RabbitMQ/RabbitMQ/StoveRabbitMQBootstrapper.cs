using System;

using MassTransit;

using Stove.Bootstrapping;

namespace Stove.RabbitMQ
{
    [DependsOn(
        typeof(StoveKernelBootstrapper)
    )]
    public class StoveRabbitMQBootstrapper : StoveBootstrapper
    {
        public override void PreStart()
        {
            StoveConfiguration.GetConfigurerIfExists<IStoveRabbitMQConfiguration>().Invoke(StoveConfiguration.Modules.StoveRabbitMQ());
        }

        public override void Start()
        {
            StoveConfiguration.Resolver.Resolve<IBusControl>().Start();
        }

        public override void Shutdown()
        {
            try
            {
                StoveConfiguration.Resolver.Resolve<IBusControl>().Stop();
            }
            catch (Exception exception)
            {
                Logger.Error($"An error occured while RabbitMQ stopping.", exception);
            }
        }
    }
}
