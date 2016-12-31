using Autofac;
using Autofac.Extras.IocManager;

using Stove.Domain.Uow;

namespace Stove.Configuration
{
    public class StoveStartupConfiguration : DictionaryBasedConfig, IStoveStartupConfiguration, IStartable
    {
        public StoveStartupConfiguration(IScopeResolver scopeResolver)
        {
            ScopeResolver = scopeResolver;
        }

        public void Start()
        {
            UnitOfWork.RegisterFilter(StoveDataFilters.SoftDelete, true);
        }

        public string DefaultNameOrConnectionString { get; set; }

        public IUnitOfWorkDefaultOptions UnitOfWork { get; set; }

        public IScopeResolver ScopeResolver { get; }

        public T Get<T>()
        {
            return GetOrCreate(typeof(T).FullName, () => ScopeResolver.Resolve<T>());
        }
    }
}
