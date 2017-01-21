using System;

using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Configuration;

using Stove.Bootstrapping;
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
            if (Resolver.IsRegistered(typeof(Func<IStoveRedisCacheConfiguration, IStoveRedisCacheConfiguration>)))
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
            redisConfiguration.Configuration = RedisCachingSectionHandler.GetConfig();

            if (redisConfiguration.Configuration == null)
            {
                throw new StoveException("There is no Redis connection string section in app.config or web.config file and define section and configurations. If it is please" +
                                         " make sure of your config file is setted as CopyAlways from it's properties.");
            }

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
