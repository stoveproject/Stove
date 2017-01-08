using System;
using System.Reflection;

using Autofac.Core;
using Autofac.Extras.IocManager;
using Autofac.Extras.IocManager.DynamicProxy;

using Stove.BackgroundJobs;
using Stove.Configuration;
using Stove.Domain.Uow;
using Stove.Events.Bus;

namespace Stove
{
    public static class StoveRegistrationExtensions
    {
        public static IIocBuilder UseStove(this IIocBuilder builder, bool autoUnitOfWorkInterceptionEnabled = false)
        {
            if (autoUnitOfWorkInterceptionEnabled)
            {
                builder.RegisterServices(r => r.UseBuilder(cb => cb.RegisterCallback(registry => registry.Registered += RegistryOnRegistered)));
            }
            RegisterDefaults(builder);
            return builder;
        }

        private static void RegistryOnRegistered(object sender, ComponentRegisteredEventArgs args)
        {
            if (UnitOfWorkHelper.IsConventionalUowClass(args.ComponentRegistration.Activator.LimitType)
                || UnitOfWorkHelper.HasUnitOfWorkAttribute(args.ComponentRegistration.Activator.LimitType))
            {
                UnitOfWorkRegistrar(args);
            }
        }

        private static void UnitOfWorkRegistrar(ComponentRegisteredEventArgs args)
        {
            args.ComponentRegistration.InterceptedBy<UnitOfWorkInterceptor>(interceptAdditionalInterfaces: true);
        }

        private static void RegisterDefaults(IIocBuilder builder)
        {
            builder.RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly()));
            builder.RegisterServices(r => r.Register<IGuidGenerator>(context => SequentialGuidGenerator.Instance));
            builder.RegisterServices(r => r.Register<IStoveStartupConfiguration, StoveStartupConfiguration>(Lifetime.Singleton));
            builder.RegisterServices(r => r.Register<IBackgroundJobConfiguration, BackgroundJobConfiguration>(Lifetime.Singleton));
            builder.RegisterServices(r => r.Register<IModuleConfigurations, ModuleConfigurations>(Lifetime.Singleton));
        }

        public static IIocBuilder UseDefaultConnectionStringResolver(this IIocBuilder builder)
        {
            builder.RegisterServices(r => r.Register<IConnectionStringResolver, DefaultConnectionStringResolver>());
            return builder;
        }

        public static IIocBuilder UseDefaultEventBus(this IIocBuilder builder)
        {
            builder.RegisterServices(r => r.Register<IEventBus>(context => EventBus.Default));
            return builder;
        }

        public static IIocBuilder UseEventBus(this IIocBuilder builder)
        {
            builder.RegisterServices(r => r.Register<IEventBus, EventBus>());
            return builder;
        }

        public static IIocBuilder UseBackgroundJobs(this IIocBuilder builder)
        {
            Func<IBackgroundJobConfiguration, IBackgroundJobConfiguration> configurer = configuration =>
            {
                configuration.IsJobExecutionEnabled = true;
                return configuration;
            };

            builder.RegisterServices(r => r.Register(ctx => configurer));
            return builder;
        }
    }
}
