using System.Runtime.Caching;

using Autofac.Extras.IocManager;

using Stove.Runtime.Caching.Configuration;

namespace Stove.Runtime.Caching.Memory
{
    /// <summary>
    ///     Implements <see cref="ICacheManager" /> to work with <see cref="MemoryCache" />.
    /// </summary>
    public class StoveMemoryCacheManager : CacheManagerBase
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        public StoveMemoryCacheManager(IScopeResolver scopeResolver, ICachingConfiguration configuration)
            : base(scopeResolver.BeginScope(), configuration)
        {
        }

        protected override ICache CreateCacheImplementation(string name)
        {
            return ScopeResolver.Resolve<StoveMemoryCache>(new { name });
        }
    }
}
