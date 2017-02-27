using JetBrains.Annotations;

using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Configuration;

namespace Stove.Redis.Redis
{
    public interface IStoveRedisCacheConfiguration
    {
        [CanBeNull]
        IRedisCachingConfiguration Configuration { get; set; }

        [CanBeNull]
        ConfigurationOptions ConfigurationOptions { get; set; }
    }
}
