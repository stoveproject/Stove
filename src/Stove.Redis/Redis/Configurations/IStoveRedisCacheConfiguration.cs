using JetBrains.Annotations;

using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Configuration;

using Stove.Configuration;

namespace Stove.Redis.Configurations
{
    public interface IStoveRedisCacheConfiguration
    {
        [CanBeNull]
        IRedisCachingConfiguration CachingConfiguration { get; set; }

        [CanBeNull]
        ConfigurationOptions ConfigurationOptions { get; set; }

        IStoveStartupConfiguration Configuration { get; }
    }
}
