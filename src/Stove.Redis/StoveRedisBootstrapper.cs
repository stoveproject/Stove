using System;

using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Configuration;

using Stove.Bootstrapping;
using Stove.Bootstrapping.Bootstrappers;
using Stove.Redis.Redis;

namespace Stove.Redis
{
    [DependsOn(
        typeof(StoveKernelBootstrapper)
    )]
    public class StoveRedisBootstrapper : StoveBootstrapper
    {
        public override void PreStart()
        {
            if (Resolver.IsRegistered<Func<IStoveRedisCacheConfiguration, IStoveRedisCacheConfiguration>>())
            {
                var redisConfigurer = Resolver.Resolve<Func<IStoveRedisCacheConfiguration, IStoveRedisCacheConfiguration>>();
                Configuration.Caching.UseRedis(options => redisConfigurer(options));
            }
            else
            {
                ConfigureRedis(Configuration.Modules.StoveRedis());
            }
        }

        private void ConfigureRedis(IStoveRedisCacheConfiguration redisConfiguration)
        {
            redisConfiguration.Configuration = RedisCachingSectionHandler.GetConfig();
            redisConfiguration.ConfigurationOptions = new ConfigurationOptions
            {
                AllowAdmin = redisConfiguration.Configuration.AllowAdmin,
                Ssl = redisConfiguration.Configuration.Ssl,
                ConnectTimeout = redisConfiguration.Configuration.ConnectTimeout,
                AbortOnConnectFail = false
            };

            foreach (RedisHost redisHost in redisConfiguration.Configuration.RedisHosts)
            {
                redisConfiguration.ConfigurationOptions.EndPoints.Add(redisHost.Host, redisHost.CachePort);
            }
        }
    }
}
