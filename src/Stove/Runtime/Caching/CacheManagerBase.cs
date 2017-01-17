using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Autofac.Extras.IocManager;

using Stove.Runtime.Caching.Configuration;

namespace Stove.Runtime.Caching
{
    /// <summary>
    ///     Base class for cache managers.
    /// </summary>
    public abstract class CacheManagerBase : ICacheManager, ISingletonDependency
    {
        protected readonly ConcurrentDictionary<string, ICache> Caches;
        protected readonly ICachingConfiguration Configuration;
        protected readonly IScopeResolver ScopeResolver;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="scopeResolver"></param>
        /// <param name="configuration"></param>
        protected CacheManagerBase(IScopeResolver scopeResolver, ICachingConfiguration configuration)
        {
            ScopeResolver = scopeResolver;
            Configuration = configuration;
            Caches = new ConcurrentDictionary<string, ICache>();
        }

        public IReadOnlyList<ICache> GetAllCaches()
        {
            return Caches.Values.ToImmutableList();
        }

        public virtual ICache GetCache(string name)
        {
            Check.NotNull(name, nameof(name));

            return Caches.GetOrAdd(name, cacheName =>
            {
                ICache cache = CreateCacheImplementation(cacheName);

                IEnumerable<ICacheConfigurator> configurators = Configuration.Configurators.Where(c => c.CacheName == null || c.CacheName == cacheName);

                foreach (ICacheConfigurator configurator in configurators)
                {
                    configurator.InitAction?.Invoke(cache);
                }

                return cache;
            });
        }

        public virtual void Dispose()
        {
            ScopeResolver.Dispose();
            Caches.Clear();
        }

        /// <summary>
        ///     Used to create actual cache implementation.
        /// </summary>
        /// <param name="name">Name of the cache</param>
        /// <returns>Cache object</returns>
        protected abstract ICache CreateCacheImplementation(string name);
    }
}
