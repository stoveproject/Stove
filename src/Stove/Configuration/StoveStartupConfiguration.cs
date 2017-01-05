using System;
using System.Collections.Generic;

using Autofac.Extras.IocManager;

using Stove.Domain.Uow;

namespace Stove.Configuration
{
    public class StoveStartupConfiguration : DictionaryBasedConfig, IStoveStartupConfiguration
    {
        public StoveStartupConfiguration(IResolver resolver)
        {
            Resolver = resolver;
            TypedConnectionStrings = new Dictionary<Type, string>();
        }

        public IResolver Resolver { get; }

        public string DefaultNameOrConnectionString { get; set; }

        public IUnitOfWorkDefaultOptions UnitOfWork { get; set; }

        public Dictionary<Type, string> TypedConnectionStrings { get; set; }

        public T Get<T>()
        {
            return GetOrCreate(typeof(T).FullName, () => Resolver.Resolve<T>());
        }
    }
}
