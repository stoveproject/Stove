using System;

using Autofac.Extras.IocManager;

using Stove.Runtime.Caching.Configuration;

namespace Stove.Redis.Redis
{
    /// <summary>
    ///     Extension methods for <see cref="ICachingConfiguration" />.
    /// </summary>
    public static class RedisCacheConfigurationExtensions
    {
        /// <summary>
        ///     Configures caching to use Redis as cache server.
        /// </summary>
        /// <param name="cachingConfiguration">The caching configuration.</param>
        public static void UseRedis(this ICachingConfiguration cachingConfiguration)
        {
            cachingConfiguration.UseRedis(options => { });
        }

        /// <summary>
        ///     Configures caching to use Redis as cache server.
        /// </summary>
        /// <param name="cachingConfiguration">The caching configuration.</param>
        /// <param name="optionsAction">Ac action to get/set options</param>
        public static void UseRedis(this ICachingConfiguration cachingConfiguration, Action<StoveRedisCacheOptions> optionsAction)
        {
            IResolver resolver = cachingConfiguration.StoveConfiguration.Resolver;
            optionsAction(resolver.Resolve<StoveRedisCacheOptions>());
        }
    }
}
