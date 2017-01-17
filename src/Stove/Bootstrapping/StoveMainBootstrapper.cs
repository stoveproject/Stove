using Autofac;
using Autofac.Extras.IocManager;

namespace Stove.Bootstrapping
{
    public class StoveMainBootstrapper : IStartable, ITransientDependency
    {
        private readonly IStoveBootstrapperManager _stoveBootstrapperManager;

        public StoveMainBootstrapper(IStoveBootstrapperManager stoveBootstrapperManager)
        {
            _stoveBootstrapperManager = stoveBootstrapperManager;
        }

        public void Start()
        {
            _stoveBootstrapperManager.StartBootstrappers();
        }
    }
}