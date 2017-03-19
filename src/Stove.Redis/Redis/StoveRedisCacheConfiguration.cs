using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Configuration;

namespace Stove.Redis
{
    public class StoveRedisCacheConfiguration : IStoveRedisCacheConfiguration
    {
        public IRedisCachingConfiguration Configuration { get; set; }

        public ConfigurationOptions ConfigurationOptions { get; set; }
    }
}
