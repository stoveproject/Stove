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
