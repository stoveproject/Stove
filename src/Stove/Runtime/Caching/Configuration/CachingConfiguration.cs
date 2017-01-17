using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using Stove.Configuration;

namespace Stove.Runtime.Caching.Configuration
{
    internal class CachingConfiguration : ICachingConfiguration
    {
        private readonly List<ICacheConfigurator> _configurators;

        public CachingConfiguration(IStoveStartupConfiguration stoveConfiguration)
        {
            StoveConfiguration = stoveConfiguration;
            _configurators = new List<ICacheConfigurator>();
        }

        public IStoveStartupConfiguration StoveConfiguration { get; }

        public IReadOnlyList<ICacheConfigurator> Configurators
        {
            get { return _configurators.ToImmutableList(); }
        }

        public void ConfigureAll(Action<ICache> initAction)
        {
            _configurators.Add(new CacheConfigurator(initAction));
        }

        public void Configure(string cacheName, Action<ICache> initAction)
        {
            _configurators.Add(new CacheConfigurator(cacheName, initAction));
        }
    }
}
