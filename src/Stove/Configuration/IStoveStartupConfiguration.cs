using System;
using System.Collections.Generic;

using Autofac.Extras.IocManager;

using Stove.BackgroundJobs;
using Stove.Domain.Uow;
using Stove.Runtime.Caching.Configuration;

namespace Stove.Configuration
{
    public interface IStoveStartupConfiguration
    {
        /// <summary>
        ///     Gets or sets the type of the starter bootstrapper.
        /// </summary>
        /// <value>
        ///     The type of the starter bootstrapper.
        /// </value>
        Type StarterBootstrapperType { get; set; }

        /// <summary>
        ///     Gets the resolver.
        /// </summary>
        /// <value>
        ///     The resolver.
        /// </value>
        IResolver Resolver { get; }

        /// <summary>
        ///     Gets/sets default connection string used by ORM module.
        ///     It can be name of a connection string in application's config file or can be full connection string.
        /// </summary>
        string DefaultNameOrConnectionString { get; set; }

        /// <summary>
        ///     Used to configure unit of work defaults.
        /// </summary>
        IUnitOfWorkDefaultOptions UnitOfWork { get; }

        /// <summary>
        ///     Gets or sets the typed connection strings.
        /// </summary>
        /// <value>
        ///     The typed connection strings.
        /// </value>
        Dictionary<Type, string> TypedConnectionStrings { get; }

        /// <summary>
        ///     Gets or sets the background jobs configuration.
        /// </summary>
        /// <value>
        ///     The background jobs.
        /// </value>
        IBackgroundJobConfiguration BackgroundJobs { get; }

        /// <summary>
        ///     Gets or sets the modules.
        /// </summary>
        /// <value>
        ///     The modules.
        /// </value>
        IModuleConfigurations Modules { get; }

        /// <summary>
        ///     Gets the caching.
        /// </summary>
        /// <value>
        ///     The caching.
        /// </value>
        ICachingConfiguration Caching { get; }

        /// <summary>
        ///     Gets a configuration object.
        /// </summary>
        T Get<T>();

        /// <summary>
        ///     Gets the configurer.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Func<T, T> GetConfigurerIfExists<T>();
    }
}
