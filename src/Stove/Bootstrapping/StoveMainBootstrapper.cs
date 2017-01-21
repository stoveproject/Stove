using System;

using Autofac;
using Autofac.Extras.IocManager;

using Stove.Configuration;

namespace Stove.Bootstrapping
{
    public class StoveMainBootstrapper : IStartable, ITransientDependency
    {
        private readonly Func<IStoveStartupConfiguration, IStoveStartupConfiguration> _configurer;
        private readonly StoveStartupConfiguration _startupConfiguration;
        private readonly IStoveBootstrapperManager _stoveBootstrapperManager;

        public StoveMainBootstrapper(
            IStoveBootstrapperManager stoveBootstrapperManager,
            Func<IStoveStartupConfiguration, IStoveStartupConfiguration> configurer,
            StoveStartupConfiguration startupConfiguration)
        {
            _stoveBootstrapperManager = stoveBootstrapperManager;
            _configurer = configurer;
            _startupConfiguration = startupConfiguration;
        }

        public void Start()
        {
            _startupConfiguration.Initialize();
            _stoveBootstrapperManager.StartBootstrappers(_configurer(_startupConfiguration).StarterBootstrapperType);
        }
    }
}
