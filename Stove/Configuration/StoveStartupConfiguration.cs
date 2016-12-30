using Autofac.Extras.IocManager;

using Stove.Domain.Uow;

namespace Stove.Configuration
{
    public class StoveStartupConfiguration : DictionaryBasedConfig, IStoveStartupConfiguration
    {
        public StoveStartupConfiguration(IIocManager iocManager)
        {
            IocManager = iocManager;
        }

        public IIocManager IocManager { get; }

        public string DefaultNameOrConnectionString { get; set; }

        public IUnitOfWorkDefaultOptions UnitOfWork { get; set; }

        public T Get<T>()
        {
            return GetOrCreate(typeof(T).FullName, () => IocManager.Resolve<T>());
        }
    }
}
