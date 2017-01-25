using System;
using System.Collections.Generic;

using Autofac.Extras.IocManager;

using Stove.BackgroundJobs;
using Stove.Dependency;
using Stove.Domain.Uow;
using Stove.Runtime.Caching.Configuration;

namespace Stove.Configuration
{
    public class StoveStartupConfiguration : DictionaryBasedConfig, IStoveStartupConfiguration
    {
        public StoveStartupConfiguration(IResolver resolver)
        {
            Resolver = resolver;
        }

        public IResolver Resolver { get; }

        public Type StarterBootstrapperType { get; set; }

        public string DefaultNameOrConnectionString { get; set; }

        public Dictionary<Type, string> TypedConnectionStrings { get; set; }

        public IModuleConfigurations Modules { get; private set; }

        public IBackgroundJobConfiguration BackgroundJobs { get; private set; }

        public IUnitOfWorkDefaultOptions UnitOfWork { get; private set; }

        public ICachingConfiguration Caching { get; private set; }

        public T Get<T>()
        {
            return GetOrCreate(typeof(T).FullName, () => Resolver.Resolve<T>());
        }

        public Func<T, T> GetConfigurerIfExists<T>()
        {
            return GetOrCreate($"{typeof(T)}.Configurer", () =>
            {
                Func<T, T> configurer;
                if (Resolver.ResolveIfExists(out configurer))
                {
                    return configurer;
                }

                return (arg => arg);
            });
        }

        public void Initialize()
        {
            Modules = Resolver.Resolve<IModuleConfigurations>();
            BackgroundJobs = Resolver.Resolve<IBackgroundJobConfiguration>();
            UnitOfWork = Resolver.Resolve<IUnitOfWorkDefaultOptions>();
            Caching = Resolver.Resolve<ICachingConfiguration>();
            TypedConnectionStrings = new Dictionary<Type, string>();
        }
    }
}
