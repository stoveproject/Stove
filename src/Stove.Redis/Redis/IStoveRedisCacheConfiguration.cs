using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Configuration;

namespace Stove.Redis.Redis
{
    public interface IStoveRedisCacheConfiguration
    {
        IRedisCachingConfiguration Configuration { get; set; }

        ConfigurationOptions ConfigurationOptions { get; set; }
    }
}
