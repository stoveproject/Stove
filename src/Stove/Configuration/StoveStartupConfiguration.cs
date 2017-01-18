using System;
using System.Collections.Generic;

using Autofac.Extras.IocManager;

using Stove.BackgroundJobs;
using Stove.Domain.Uow;
using Stove.Runtime.Caching.Configuration;

namespace Stove.Configuration
{
    public class StoveStartupConfiguration : DictionaryBasedConfig, IStoveStartupConfiguration
    {
        public StoveStartupConfiguration(IResolver resolver)
        {
            Resolver = resolver;

            TypedConnectionStrings = new Dictionary<Type, string>();
        }

        public IModuleConfigurations Modules { get; set; }

        public IBackgroundJobConfiguration BackgroundJobs { get; set; }

        public IResolver Resolver { get; }

        public string DefaultNameOrConnectionString { get; set; }

        public IUnitOfWorkDefaultOptions UnitOfWork { get; set; }

        public Dictionary<Type, string> TypedConnectionStrings { get; set; }

        public ICachingConfiguration Caching { get; set; }

        public T Get<T>()
        {
            return GetOrCreate(typeof(T).FullName, () => Resolver.Resolve<T>());
        }
    }
}
