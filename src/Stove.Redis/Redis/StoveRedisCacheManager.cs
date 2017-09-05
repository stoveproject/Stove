using Autofac.Extras.IocManager;

using JetBrains.Annotations;

using Stove.Runtime.Caching;
using Stove.Runtime.Caching.Configuration;

namespace Stove.Redis
{
	/// <summary>
	///     Used to create <see cref="StoveRedisCache" /> instances.
	/// </summary>
	public class StoveRedisCacheManager : CacheManagerBase
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="StoveRedisCacheManager" /> class.
		/// </summary>
		public StoveRedisCacheManager([NotNull] IScopeResolver scopeResolver, [NotNull] ICachingConfiguration configuration)
			: base(scopeResolver.BeginScope(), configuration)
		{
		}

		protected override ICache CreateCacheImplementation(string name)
		{
			return ScopeResolver.Resolve<StoveRedisCache>(new { name });
		}
	}
}
