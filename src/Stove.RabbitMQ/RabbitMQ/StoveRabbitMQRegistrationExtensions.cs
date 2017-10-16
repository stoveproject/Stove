using System;

using Autofac;
using Autofac.Extras.IocManager;

using GreenPipes;

using JetBrains.Annotations;

using MassTransit;
using MassTransit.RabbitMqTransport;

using Stove.MQ;
using Stove.Reflection.Extensions;

namespace Stove.RabbitMQ
{
    public static class StoveRabbitMQRegistrationExtensions
    {
        /// <summary>
        ///     Uses the Stove RabbitMq integration with producer and consumer.Loads consumers from Ioc.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="rabbitMQConfigurer">The rabbit mq configurer.</param>
        /// <param name="busConfigurer"></param>
        /// <returns></returns>
        [NotNull]
        public static IIocBuilder UseStoveRabbitMQ(
            [NotNull] this IIocBuilder builder,
            [NotNull] Func<IStoveRabbitMQConfiguration, IStoveRabbitMQConfiguration> rabbitMQConfigurer,
            Action<IRabbitMqBusFactoryConfigurator> busConfigurer)
        {
            Check.NotNull(rabbitMQConfigurer, nameof(rabbitMQConfigurer));

            builder
                .RegisterServices(r =>
                {
                    r.RegisterAssemblyByConvention(typeof(StoveRabbitMQRegistrationExtensions).GetAssembly());
                    r.Register<IStoveRabbitMQConfiguration, StoveRabbitMQConfiguration>(Lifetime.Singleton);
                    r.Register<IMessageBus, StoveRabbitMQMessageBus>();
                    r.Register(ctx => rabbitMQConfigurer);
                });

            builder.RegisterServices(r => r.UseBuilder(cb =>
            {
                cb.Register(ctx =>
                  {
                      var configuration = ctx.Resolve<IStoveRabbitMQConfiguration>();

                      IBusControl busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
                      {
                          IRabbitMqHost host = EnsureStoveDefaults(cfg, configuration);

                          cfg.ReceiveEndpoint(host, configuration.QueueName, ec => { ec.LoadFrom(ctx); });

                          busConfigurer?.Invoke(cfg);
                      });

                      return busControl;
                  }).SingleInstance()
                  .As<IBusControl>()
                  .As<IBus>();
            }));

            return builder;
        }

        /// <summary>
        ///     Uses the Stove RabbitMQ integration as just publisher.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="rabbitMQConfigurer">The rabbit mq configurer.</param>
        /// <returns></returns>
        [NotNull]
        public static IIocBuilder UseStoveRabbitMQ(
            [NotNull] this IIocBuilder builder,
            [NotNull] Func<IStoveRabbitMQConfiguration, IStoveRabbitMQConfiguration> rabbitMQConfigurer)
        {
            Check.NotNull(rabbitMQConfigurer, nameof(rabbitMQConfigurer));

            builder
                .RegisterServices(r =>
                {
                    r.RegisterAssemblyByConvention(typeof(StoveRabbitMQRegistrationExtensions).GetAssembly());
                    r.Register<IStoveRabbitMQConfiguration, StoveRabbitMQConfiguration>(Lifetime.Singleton);
                    r.Register<IMessageBus, StoveRabbitMQMessageBus>();
                    r.Register(ctx => rabbitMQConfigurer);
                });

            builder.RegisterServices(r => r.UseBuilder(cb =>
            {
                cb.Register(ctx =>
                  {
                      var configuration = ctx.Resolve<IStoveRabbitMQConfiguration>();

                      IBusControl busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
                      {
                          EnsureStoveDefaults(cfg, configuration);
                      });

                      return busControl;
                  }).SingleInstance()
                  .As<IBusControl>()
                  .As<IBus>();
            }));

            return builder;
        }

        /// <summary>
        ///     Uses the Stove RabbitMQ integration. Consumer loadings doesn't come from IoC, you should register explicitly with
        ///     <see cref="consumerConfigurer" />
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="rabbitMQConfigurer">The rabbit mq configurer.</param>
        /// <param name="consumerConfigurer"></param>
        /// <returns></returns>
        [NotNull]
        public static IIocBuilder UseStoveRabbitMQ(
            [NotNull] this IIocBuilder builder,
            [NotNull] Func<IStoveRabbitMQConfiguration, IStoveRabbitMQConfiguration> rabbitMQConfigurer,
            Action<IRabbitMqHost, IRabbitMqBusFactoryConfigurator, IComponentContext> consumerConfigurer
        )
        {
            Check.NotNull(rabbitMQConfigurer, nameof(rabbitMQConfigurer));
            Check.NotNull(consumerConfigurer, nameof(consumerConfigurer));

            builder
                .RegisterServices(r =>
                {
                    r.RegisterAssemblyByConvention(typeof(StoveRabbitMQRegistrationExtensions).GetAssembly());
                    r.Register<IStoveRabbitMQConfiguration, StoveRabbitMQConfiguration>(Lifetime.Singleton);
                    r.Register<IMessageBus, StoveRabbitMQMessageBus>();
                    r.Register(ctx => rabbitMQConfigurer);
                });

            builder.RegisterServices(r => r.UseBuilder(cb =>
            {
                cb.Register(ctx =>
                  {
                      var configuration = ctx.Resolve<IStoveRabbitMQConfiguration>();

                      IBusControl busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
                      {
                          IRabbitMqHost host = EnsureStoveDefaults(cfg, configuration);

                          consumerConfigurer(host, cfg, ctx);
                      });

                      return busControl;
                  }).SingleInstance()
                  .As<IBusControl>()
                  .As<IBus>();
            }));

            return builder;
        }

        private static IRabbitMqHost EnsureStoveDefaults(IRabbitMqBusFactoryConfigurator cfg, IStoveRabbitMQConfiguration configuration)
        {
            IRabbitMqHost host = cfg.Host(new Uri(configuration.HostAddress), h =>
            {
                h.Username(configuration.Username);
                h.Password(configuration.Password);
            });

            if (configuration.UseRetryMechanism) cfg.UseRetry(rtryConf => { rtryConf.Immediate(configuration.MaxRetryCount); });

            if (configuration.PrefetchCount.HasValue) cfg.PrefetchCount = (ushort)configuration.PrefetchCount;

            if (configuration.ConcurrencyLimit.HasValue) cfg.UseConcurrencyLimit(configuration.ConcurrencyLimit.Value);

            return host;
        }
    }
}
