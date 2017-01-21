using System;
using System.Reflection;

using Autofac;
using Autofac.Extras.IocManager;

using MassTransit;
using MassTransit.RabbitMqTransport;

using Stove.MQ;
using Stove.RabbitMQ.RabbitMQ;

namespace Stove.RabbitMQ
{
    public static class StoveRabbitMQRegistrationExtensions
    {
        public static IIocBuilder UseStoveRabbitMQ(this IIocBuilder builder, Func<IStoveRabbitMQConfiguration, IStoveRabbitMQConfiguration> rabbitMQConfigurer = null)
        {
            builder
                .RegisterServices(r =>
                {
                    r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
                    r.Register<IStoveRabbitMQConfiguration, StoveRabbitMQConfiguration>(Lifetime.Singleton);
                    r.Register<IMessageBus, RabbitMQMessageBus>();

                    if (rabbitMQConfigurer != null)
                    {
                        r.Register(ctx => rabbitMQConfigurer);
                    }
                });

            builder.RegisterServices(r => r.UseBuilder(cb =>
            {
                cb.Register(ctx =>
                  {
                      var busConfiguration = ctx.Resolve<IStoveRabbitMQConfiguration>();

                      IBusControl busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
                      {
                          IRabbitMqHost host = cfg.Host(new Uri(busConfiguration.HostAddress), h =>
                          {
                              h.Username(busConfiguration.Username);
                              h.Password(busConfiguration.Password);
                          });

                          cfg.ReceiveEndpoint(busConfiguration.QueueName, ec => { ec.LoadFrom(ctx); });
                      });

                      return busControl;
                  }).SingleInstance()
                  .As<IBusControl>()
                  .As<IBus>();
                ;
            }));

            return builder;
        }
    }
}
