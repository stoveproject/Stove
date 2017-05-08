using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Configuration;

using Stove.Bootstrapping;
using Stove.Redis;
using Stove.Redis.Configurations;

namespace Stove
{
    [DependsOn(
        typeof(StoveKernelBootstrapper)
    )]
    public class StoveRedisBootstrapper : StoveBootstrapper
    {
        public override void PreStart()
        {
            if (Configuration.IsConfigurerRegistered<IStoveRedisCacheConfiguration>())
            {
                Configuration.GetConfigurerIfExists<IStoveRedisCacheConfiguration>().Invoke(Configuration.Modules.StoveRedis());
            }
            else
            {
                ConfigureRedis(Configuration.Modules.StoveRedis());
            }
        }

        private void ConfigureRedis(IStoveRedisCacheConfiguration redisConfiguration)
        {
            redisConfiguration.CachingConfiguration = RedisCachingSectionHandler.GetConfig();

            if (redisConfiguration.CachingConfiguration == null)
            {
                throw new StoveException("There is no Redis connection string section in app.config or web.config file, " +
                                         "please define redisCacheClient section and configurations. " +
                                         "If it is, please make sure of your config file is setted as CopyAlways from it's properties.");
            }

            redisConfiguration.ConfigurationOptions = new ConfigurationOptions
            {
                AllowAdmin = redisConfiguration.CachingConfiguration.AllowAdmin,
                Ssl = redisConfiguration.CachingConfiguration.Ssl,
                ConnectTimeout = redisConfiguration.CachingConfiguration.ConnectTimeout,
                AbortOnConnectFail = false
            };

            foreach (RedisHost redisHost in redisConfiguration.CachingConfiguration.RedisHosts)
            {
                redisConfiguration.ConfigurationOptions.EndPoints.Add(redisHost.Host, redisHost.CachePort);
            }
        }
    }
}
