using JetBrains.Annotations;

using Stove.Configuration;
using Stove.Runtime.Caching.Configuration;

namespace Stove.Redis.Configurations
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
        [NotNull]
        public static IStoveRedisCacheConfiguration StoveRedis([NotNull] this IModuleConfigurations configurations)
        {
            return configurations.StoveConfiguration.Get<IStoveRedisCacheConfiguration>();
        }
    }
}
