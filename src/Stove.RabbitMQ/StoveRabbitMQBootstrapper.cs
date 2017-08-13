using System;

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
            StoveConfiguration.GetConfigurerIfExists<IStoveRabbitMQConfiguration>().Invoke(StoveConfiguration.Modules.StoveRabbitMQ());
        }

        public override void PostStart()
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
