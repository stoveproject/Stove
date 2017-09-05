using Autofac;
using Autofac.Extras.IocManager;

using MassTransit;

using Stove.MQ;
using Stove.Reflection.Extensions;
using Stove.TestBase;

namespace Stove.RabbitMQ.Tests
{
    public class RabbitMQApplicationTestBase : ApplicationTestBase<StoveRabbitMQBootstrapper>
    {
        protected RabbitMQApplicationTestBase()
        {
            Building(builder => { UseStoveRabbitMQInMemory(builder).RegisterServices(r =>
            {
                r.RegisterAssemblyByConvention(typeof(RabbitMQApplicationTestBase).GetAssembly());
            }); });
        }

        private IIocBuilder UseStoveRabbitMQInMemory(IIocBuilder builder)
        {
            builder
                .RegisterServices(r =>
                {
                    r.RegisterType<StoveRabbitMQBootstrapper>();
                    r.Register<IStoveRabbitMQConfiguration, StoveRabbitMQConfiguration>(Lifetime.Singleton);
                    r.Register<IMessageBus, StoveRabbitMQMessageBus>(Lifetime.Singleton);
                });

            builder.RegisterServices(r => r.UseBuilder(cb =>
            {
                cb.Register(ctx =>
                {
                    return Bus.Factory.CreateUsingInMemory(configurator =>
                    {
                        configurator.ReceiveEndpoint("test", endpointConfigurator => { endpointConfigurator.LoadFrom(ctx); });
                    });
                }).As<IBusControl>().As<IBus>().SingleInstance();
            }));

            return builder;
        }
    }
}
