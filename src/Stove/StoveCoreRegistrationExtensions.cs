using System;
using System.Reflection;

using Autofac.Core;
using Autofac.Extras.IocManager;
using Autofac.Extras.IocManager.DynamicProxy;

using JetBrains.Annotations;

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
using Stove.Runtime;
using Stove.Runtime.Caching.Configuration;
using Stove.Runtime.Caching.Memory;
using Stove.Runtime.Remoting;

namespace Stove
{
    public static class StoveCoreRegistrationExtensions
    {
        [NotNull]
        public static IIocBuilder UseStove<TStarterBootstrapper>([NotNull] this IIocBuilder builder, bool autoUnitOfWorkInterceptionEnabled = false)
            where TStarterBootstrapper : StoveBootstrapper
        {
            return UseStove(builder, typeof(TStarterBootstrapper), autoUnitOfWorkInterceptionEnabled);
        }

        [NotNull]
        public static IIocBuilder UseStove([NotNull] this IIocBuilder builder, [NotNull] Type starterBootstrapperType, bool autoUnitOfWorkInterceptionEnabled = false)
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
            RegisterStoveDefaults(builder);

            return builder;
        }

        [NotNull]
        public static IIocBuilder UseStoveDefaultConnectionStringResolver([NotNull] this IIocBuilder builder)
        {
            builder.RegisterServices(r => r.Register<IConnectionStringResolver, DefaultConnectionStringResolver>());
            return builder;
        }

        [NotNull]
        public static IIocBuilder UseStoveDefaultEventBus([NotNull] this IIocBuilder builder)
        {
            builder.RegisterServices(r => r.Register<IEventBus>(context => EventBus.Default));
            return builder;
        }

        [NotNull]
        public static IIocBuilder UseStoveEventBus([NotNull] this IIocBuilder builder)
        {
            builder.RegisterServices(r => r.Register<IEventBus, EventBus>());
            return builder;
        }

        [NotNull]
        public static IIocBuilder UseStoveBackgroundJobs([NotNull] this IIocBuilder builder)
        {
            Func<IBackgroundJobConfiguration, IBackgroundJobConfiguration> configurer = configuration =>
            {
                configuration.IsJobExecutionEnabled = true;
                return configuration;
            };

            builder.RegisterServices(r => r.Register(ctx => configurer));
            return builder;
        }

        [NotNull]
        public static IIocBuilder UseStoveNullEventBus([NotNull] this IIocBuilder builder)
        {
            builder.RegisterServices(r => r.Register<IEventBus>(context => NullEventBus.Instance, keepDefault: true));
            return builder;
        }

        [NotNull]
        public static IIocBuilder UseStoveNullObjectMapper([NotNull] this IIocBuilder builder)
        {
            builder.RegisterServices(r => r.Register<IObjectMapper>(context => NullObjectMapper.Instance, keepDefault: true));
            return builder;
        }

        [NotNull]
        public static IIocBuilder UseStoveNullUnitOfWork([NotNull] this IIocBuilder builder)
        {
            builder.RegisterServices(r => r.Register<IUnitOfWork, NullUnitOfWork>(keepDefault: true));
            return builder;
        }

        [NotNull]
        public static IIocBuilder UseStoveNullUnitOfWorkFilterExecuter([NotNull] this IIocBuilder builder)
        {
            builder.RegisterServices(r => r.Register<IUnitOfWorkFilterExecuter, NullUnitOfWorkFilterExecuter>(keepDefault: true));
            return builder;
        }

        [NotNull]
        public static IIocBuilder UseStoveNullEntityChangedEventHelper([NotNull] this IIocBuilder builder)
        {
            builder.RegisterServices(r => r.Register<IEntityChangeEventHelper, NullEntityChangeEventHelper>(keepDefault: true));
            return builder;
        }

        [NotNull]
        public static IIocBuilder UseStoveNullAsyncQueryableExecuter([NotNull] this IIocBuilder builder)
        {
            builder.RegisterServices(r => r.Register<IAsyncQueryableExecuter, NullAsyncQueryableExecuter>(keepDefault: true));
            return builder;
        }

        [NotNull]
        public static IIocBuilder UseStoveNullMessageBus([NotNull] this IIocBuilder builder)
        {
            return builder.RegisterServices(r => r.Register<IMessageBus>(ctx => NullMessageBus.Instance, keepDefault: true));
        }

        [NotNull]
        public static IIocBuilder UseStoveNullLogger([NotNull] this IIocBuilder builder)
        {
            return builder.RegisterServices(r => r.Register<ILogger, NullLogger>(keepDefault: true));
        }

        [NotNull]
        public static IIocBuilder UseStoveMemoryCaching([NotNull] this IIocBuilder builder)
        {
            return builder.RegisterServices(r => r.RegisterType<StoveMemoryCache>(keepDefault: true));
        }

        /// <summary>
        ///     Uses the Stove with nullables, prefer when you writing unit tests.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="starterBootstrapperType"></param>
        /// <param name="autoUnitOfWorkInterceptionEnabled"></param>
        /// <returns></returns>
        [NotNull]
        public static IIocBuilder UseStoveWithNullables([NotNull] this IIocBuilder builder, [NotNull] Type starterBootstrapperType, bool autoUnitOfWorkInterceptionEnabled = false)
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

        private static void RegistryOnRegistered([CanBeNull] object sender, [NotNull] ComponentRegisteredEventArgs args)
        {
            if (UnitOfWorkHelper.IsConventionalUowClass(args.ComponentRegistration.Activator.LimitType)
                || UnitOfWorkHelper.HasUnitOfWorkAttribute(args.ComponentRegistration.Activator.LimitType))
            {
                UnitOfWorkRegistrar(args);
            }
        }

        private static void UnitOfWorkRegistrar([NotNull] ComponentRegisteredEventArgs args)
        {
            args.ComponentRegistration.InterceptedBy<UnitOfWorkInterceptor>(true);
        }

        [NotNull]
        private static void RegisterStoveDefaults([NotNull] this IIocBuilder builder)
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
                   .RegisterServices(r => r.Register<ITypeFinder, TypeFinder>(Lifetime.Singleton))
                   .RegisterServices(r => r.RegisterGeneric(typeof(IAmbientScopeProvider<>), typeof(DataContextAmbientScopeProvider<>)));
        }
    }
}
