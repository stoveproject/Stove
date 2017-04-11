using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Configuration;

using Stove.Configuration;

namespace Stove.Redis.Configurations
{
    public class StoveRedisCacheConfiguration : IStoveRedisCacheConfiguration
    {
        public StoveRedisCacheConfiguration(IStoveStartupConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IRedisCachingConfiguration CachingConfiguration { get; set; }

        public ConfigurationOptions ConfigurationOptions { get; set; }

        public IStoveStartupConfiguration Configuration { get; }
    }
}
