using System;

using Stove.Configuration;
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
        public static void UseRedis(this ICachingConfiguration cachingConfiguration, Action<IStoveRedisCacheConfiguration> optionsAction)
        {
            optionsAction(cachingConfiguration.StoveConfiguration.Modules.StoveRedis());
        }

        /// <summary>
        ///     Stove Redis configuration accessor.
        /// </summary>
        /// <param name="configurations">The configurations.</param>
        /// <returns></returns>
        public static IStoveRedisCacheConfiguration StoveRedis(this IModuleConfigurations configurations)
        {
            return configurations.StoveConfiguration.Get<IStoveRedisCacheConfiguration>();
        }
    }
}
