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
using Stove.Events.Bus.Entities;
using Stove.Linq;
using Stove.Log;
using Stove.MQ;
using Stove.ObjectMapping;
using Stove.Reflection;
using Stove.Runtime.Caching.Configuration;
using Stove.Runtime.Caching.Memory;

namespace Stove
{
    public static class StoveCoreRegistrationExtensions
    {
        public static IIocBuilder UseStove<TStarterBootstrapper>(this IIocBuilder builder, bool autoUnitOfWorkInterceptionEnabled = false)
            where TStarterBootstrapper : StoveBootstrapper
        {
            return UseStove(builder, typeof(TStarterBootstrapper), autoUnitOfWorkInterceptionEnabled);
        }

        public static IIocBuilder UseStove(this IIocBuilder builder, Type starterBootstrapperType, bool autoUnitOfWorkInterceptionEnabled = false)
        {
            if (autoUnitOfWorkInterceptionEnabled)
            {
                builder.RegisterServices(r => r.UseBuilder(cb => cb.RegisterCallback(registry => registry.Registered += RegistryOnRegistered)));
            }

            Func<IStoveStartupConfiguration, IStoveStartupConfiguration> starterBootstrapperConfigurer = configuration =>
            {
                configuration.StarterBootstrapperType = starterBootstrapperType;
                return configuration;
            };

            builder.RegisterServices(r => r.Register(ctx => starterBootstrapperConfigurer));
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

        public static IIocBuilder UseStoveNullEventBus(this IIocBuilder builder)
        {
            builder.RegisterServices(r => r.Register<IEventBus>(context => NullEventBus.Instance));
            return builder;
        }

        public static IIocBuilder UseStoveNullObjectMapper(this IIocBuilder builder)
        {
            builder.RegisterServices(r => r.Register<IObjectMapper>(context => NullObjectMapper.Instance));
            return builder;
        }

        public static IIocBuilder UseStoveNullUnitOfWork(this IIocBuilder builder)
        {
            builder.RegisterServices(r => r.Register<IUnitOfWork, NullUnitOfWork>());
            return builder;
        }

        public static IIocBuilder UseStoveNullUnitOfWorkFilterExecuter(this IIocBuilder builder)
        {
            builder.RegisterServices(r => r.Register<IUnitOfWorkFilterExecuter, NullUnitOfWorkFilterExecuter>());
            return builder;
        }

        public static IIocBuilder UseStoveNullEntityChangedEventHelper(this IIocBuilder builder)
        {
            builder.RegisterServices(r => r.Register<IEntityChangeEventHelper, NullEntityChangeEventHelper>());
            return builder;
        }

        public static IIocBuilder UseStoveNullAsyncQueryableExecuter(this IIocBuilder builder)
        {
            builder.RegisterServices(r => r.Register<IAsyncQueryableExecuter, NullAsyncQueryableExecuter>());
            return builder;
        }

        public static IIocBuilder UseStoveNullMessageBus(this IIocBuilder builder)
        {
            return builder.RegisterServices(r => r.Register<IMessageBus>(ctx => NullMessageBus.Instance));
        }

        public static IIocBuilder UseStoveMemoryCaching(this IIocBuilder builder)
        {
            return builder.RegisterServices(r => r.RegisterType<StoveMemoryCache>(keepDefault: true));
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

        /// <summary>
        ///     Uses the Stove with nullables, prefer when you writing unit tests.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="starterBootstrapperType"></param>
        /// <param name="autoUnitOfWorkInterceptionEnabled"></param>
        /// <returns></returns>
        public static IIocBuilder UseStoveWithNullables(this IIocBuilder builder, Type starterBootstrapperType, bool autoUnitOfWorkInterceptionEnabled = false)
        {
            return builder.UseStove(starterBootstrapperType, autoUnitOfWorkInterceptionEnabled)
                          .UseStoveNullLogger()
                          .UseStoveNullEventBus()
                          .UseStoveNullObjectMapper()
                          .UseStoveNullUnitOfWork()
                          .UseStoveNullUnitOfWorkFilterExecuter()
                          .UseStoveNullEntityChangedEventHelper()
                          .UseStoveNullAsyncQueryableExecuter()
                          .UseStoveNullMessageBus();
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
                   .RegisterServices(r => r.Register<IStoveBootstrapperManager, StoveBootstrapperManager>(Lifetime.Singleton))
                   .RegisterServices(r => r.Register<IUnitOfWorkDefaultOptions, UnitOfWorkDefaultOptions>(Lifetime.Singleton))
                   .RegisterServices(r => r.Register<IStoveAssemblyFinder, StoveAssemblyFinder>(Lifetime.Singleton))
                   .RegisterServices(r => r.Register<ICachingConfiguration, CachingConfiguration>(Lifetime.Singleton))
                   .RegisterServices(r => r.Register<ITypeFinder, TypeFinder>(Lifetime.Singleton));
        }
    }
}
