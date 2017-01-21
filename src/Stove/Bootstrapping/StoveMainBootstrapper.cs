using Autofac;
using Autofac.Extras.IocManager;

using Stove.Configuration;

namespace Stove.Bootstrapping
{
    public class StoveMainBootstrapper : IStartable, ITransientDependency
    {
        private readonly StoveStartupConfiguration _startupConfiguration;
        private readonly IStoveBootstrapperManager _stoveBootstrapperManager;

        public StoveMainBootstrapper(IStoveBootstrapperManager stoveBootstrapperManager, StoveStartupConfiguration startupConfiguration)
        {
            _stoveBootstrapperManager = stoveBootstrapperManager;
            _startupConfiguration = startupConfiguration;
        }

        public void Start()
        {
            _startupConfiguration.Initialize();
            _startupConfiguration.GetConfigurerIfExists<IStoveStartupConfiguration>()(_startupConfiguration);
            _stoveBootstrapperManager.StartBootstrappers(_startupConfiguration.StarterBootstrapperType);
        }
    }
}
