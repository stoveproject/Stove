using System;
using System.Reflection;

using Autofac.Core;
using Autofac.Extras.IocManager;
using Autofac.Extras.IocManager.DynamicProxy;

using Stove.BackgroundJobs;
using Stove.Bootstrapping;
using Stove.Configuration;
using Stove.Domain.Uow;
using Stove.Events.Bus;
using Stove.Log;

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

        public static IIocBuilder UseStoveDefaultConnectionStringResolver(this IIocBuilder builder)
        {
            builder.RegisterServices(r => r.Register<IConnectionStringResolver, DefaultConnectionStringResolver>());
            return builder;
        }

        public static IIocBuilder UseStoveDefaultEventBus(this IIocBuilder builder)
        {
            builder.RegisterServices(r => r.Register<IEventBus>(context => EventBus.Default));
            return builder;
        }

        public static IIocBuilder UseStoveEventBus(this IIocBuilder builder)
        {
            builder.RegisterServices(r => r.Register<IEventBus, EventBus>());
            return builder;
        }

        public static IIocBuilder UseStoveBackgroundJobs(this IIocBuilder builder)
        {
            Func<IBackgroundJobConfiguration, IBackgroundJobConfiguration> configurer = configuration =>
            {
                configuration.IsJobExecutionEnabled = true;
                return configuration;
            };

            builder.RegisterServices(r => r.Register(ctx => configurer));
            return builder;
        }

        public static IIocBuilder UseStoveNullLogger(this IIocBuilder builder)
        {
            return builder.RegisterServices(r => r.Register<ILogger, NullLogger>());
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
            args.ComponentRegistration.InterceptedBy<UnitOfWorkInterceptor>(true);
        }

        private static void RegisterDefaults(IIocBuilder builder)
        {
            builder.RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly()))
                   .RegisterServices(r => r.Register<IGuidGenerator>(context => SequentialGuidGenerator.Instance, Lifetime.Singleton))
                   .RegisterServices(r => r.Register<IStoveStartupConfiguration, StoveStartupConfiguration>(Lifetime.Singleton))
                   .RegisterServices(r => r.Register<IBackgroundJobConfiguration, BackgroundJobConfiguration>(Lifetime.Singleton))
                   .RegisterServices(r => r.Register<IModuleConfigurations, ModuleConfigurations>(Lifetime.Singleton))
                   .RegisterServices(r => r.Register<IBootstrapperManager, BootstrapperManager>(Lifetime.Singleton))
                   .RegisterServices(r => r.Register<IUnitOfWorkDefaultOptions, UnitOfWorkDefaultOptions>(Lifetime.Singleton));
        }
    }
}
