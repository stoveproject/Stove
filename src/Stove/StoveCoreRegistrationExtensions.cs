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
        /// <summary>
        ///     Uses the stove.
        /// </summary>
        /// <typeparam name="TStarterBootstrapper">The type of the starter bootstrapper.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="autoUnitOfWorkInterceptionEnabled">if set to <c>true</c> [automatic unit of work interception enabled].</param>
        /// <returns></returns>
        [NotNull]
        public static IIocBuilder UseStove<TStarterBootstrapper>([NotNull] this IIocBuilder builder, bool autoUnitOfWorkInterceptionEnabled = false)
            where TStarterBootstrapper : StoveBootstrapper
        {
            return UseStove(builder, typeof(TStarterBootstrapper), autoUnitOfWorkInterceptionEnabled);
        }

        /// <summary>
        ///     Uses the stove.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="starterBootstrapperType">Type of the starter bootstrapper.</param>
        /// <param name="autoUnitOfWorkInterceptionEnabled">if set to <c>true</c> [automatic unit of work interception enabled].</param>
        /// <returns></returns>
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

        /// <summary>
        ///     Uses the stove default connection string resolver.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        [NotNull]
        public static IIocBuilder UseStoveDefaultConnectionStringResolver([NotNull] this IIocBuilder builder)
        {
            builder.RegisterServices(r => r.Register<IConnectionStringResolver, DefaultConnectionStringResolver>());
            return builder;
        }

        /// <summary>
        ///     Uses the stove event bus.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        [NotNull]
        public static IIocBuilder UseStoveEventBus([NotNull] this IIocBuilder builder)
        {
            return builder.RegisterServices(r => r.Register<IEventBus, EventBus>(Lifetime.Singleton));
        }

        /// <summary>
        ///     Uses the stove default event bus.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        [NotNull]
        public static IIocBuilder UseStoveDefaultEventBus([NotNull] this IIocBuilder builder)
        {
            return builder.RegisterServices(r => r.Register<IEventBus>(context => EventBus.Default, Lifetime.Singleton));
        }

        /// <summary>
        ///     Uses the stove background jobs.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
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

        /// <summary>
        ///     Uses the stove null event bus.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        [NotNull]
        public static IIocBuilder UseStoveNullEventBus([NotNull] this IIocBuilder builder)
        {
            return builder.RegisterServices(r => r.Register<IEventBus>(context => NullEventBus.Instance, keepDefault: true));
        }

        /// <summary>
        ///     Uses the stove null object mapper.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        [NotNull]
        public static IIocBuilder UseStoveNullObjectMapper([NotNull] this IIocBuilder builder)
        {
            return builder.RegisterServices(r => r.Register<IObjectMapper>(context => NullObjectMapper.Instance, keepDefault: true));
        }

        /// <summary>
        ///     Uses the stove null unit of work.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        [NotNull]
        public static IIocBuilder UseStoveNullUnitOfWork([NotNull] this IIocBuilder builder)
        {
            return builder.RegisterServices(r => r.Register<IUnitOfWork, NullUnitOfWork>(keepDefault: true));
        }

        /// <summary>
        ///     Uses the stove null unit of work filter executer.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        [NotNull]
        public static IIocBuilder UseStoveNullUnitOfWorkFilterExecuter([NotNull] this IIocBuilder builder)
        {
            return builder.RegisterServices(r => r.Register<IUnitOfWorkFilterExecuter, NullUnitOfWorkFilterExecuter>(keepDefault: true));
        }

        /// <summary>
        ///     Uses the stove null entity changed event helper.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        [NotNull]
        public static IIocBuilder UseStoveNullEntityChangedEventHelper([NotNull] this IIocBuilder builder)
        {
            return builder.RegisterServices(r => r.Register<IEntityChangeEventHelper, NullEntityChangeEventHelper>(keepDefault: true));
        }

        /// <summary>
        ///     Uses the stove null asynchronous queryable executer.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        [NotNull]
        public static IIocBuilder UseStoveNullAsyncQueryableExecuter([NotNull] this IIocBuilder builder)
        {
            return builder.RegisterServices(r => r.Register<IAsyncQueryableExecuter, NullAsyncQueryableExecuter>(keepDefault: true));
        }

        /// <summary>
        ///     Uses the stove null message bus.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        [NotNull]
        public static IIocBuilder UseStoveNullMessageBus([NotNull] this IIocBuilder builder)
        {
            return builder.RegisterServices(r => r.Register<IMessageBus>(ctx => NullMessageBus.Instance, keepDefault: true));
        }

        /// <summary>
        ///     Uses the stove null logger.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        [NotNull]
        public static IIocBuilder UseStoveNullLogger([NotNull] this IIocBuilder builder)
        {
            return builder.RegisterServices(r => r.Register<ILogger, NullLogger>(keepDefault: true));
        }

        /// <summary>
        ///     Uses the stove memory caching.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        [NotNull]
        public static IIocBuilder UseStoveMemoryCaching([NotNull] this IIocBuilder builder)
        {
            return builder.RegisterServices(r => r.RegisterType<StoveMemoryCache>(keepDefault: true));
        }

        /// <summary>
        ///     Uses only AsyncLocal based Uow provider.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        [NotNull]
        public static IIocBuilder UseStoveAsyncLocalUnitOfWorkProvider([NotNull] this IIocBuilder builder)
        {
            return builder.RegisterServices(r => r.Register<ICurrentUnitOfWorkProvider, AsyncLocalCurrentUnitOfWorkProvider>());
        }

        /// <summary>
        ///     Uses the Stove with nullables, prefer when you writing unit tests.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="starterBootstrapperType">Type of the starter bootstrapper.</param>
        /// <param name="autoUnitOfWorkInterceptionEnabled">if set to <c>true</c> [automatic unit of work interception enabled].</param>
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
                          .UseStoveNullMessageBus()
                          .UseStoveDefaultConnectionStringResolver();
        }

        /// <summary>
        ///     Uses the Stove with nullables, prefer when you writing unit tests.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="starterBootstrapperType">Type of the starter bootstrapper.</param>
        /// <param name="autoUnitOfWorkInterceptionEnabled">if set to <c>true</c> [automatic unit of work interception enabled].</param>
        /// <returns></returns>
        [NotNull]
        public static IIocBuilder UseStoveWithNullables<TStarterBootstrapper>([NotNull] this IIocBuilder builder, bool autoUnitOfWorkInterceptionEnabled = false)
        {
            return UseStoveWithNullables(builder, typeof(TStarterBootstrapper), autoUnitOfWorkInterceptionEnabled);
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
            builder.RegisterServices(r => r.RegisterAssemblyByConvention(typeof(StoveCoreRegistrationExtensions).GetTypeInfo().Assembly))
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

            builder.RegisterServices(r => r.OnDisposing += (sender, args) => { args.Context.Resolver.Resolve<IStoveBootstrapperManager>().ShutdownBootstrappers(); });
        }
    }
}
