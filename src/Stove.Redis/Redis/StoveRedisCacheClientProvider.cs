using System;

using Autofac.Extras.IocManager;

using JetBrains.Annotations;

using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core;

namespace Stove.Redis.Redis
{
    /// <summary>
    ///     Implements <see cref="IStoveRedisCacheClientProvider" />.
    /// </summary>
    public class StoveRedisCacheClientProvider : IStoveRedisCacheClientProvider, ISingletonDependency
    {
        private readonly IStoveRedisCacheConfiguration _configuration;
        private readonly Lazy<ConnectionMultiplexer> _connectionMultiplexer;
        private readonly RedisSerializer _redisSerializer;

        /// <summary>
        ///     Initializes a new instance of the <see cref="StoveRedisCacheClientProvider" /> class.
        /// </summary>
        public StoveRedisCacheClientProvider([NotNull] IStoveRedisCacheConfiguration configuration, [NotNull] RedisSerializer redisSerializer)
        {
            _configuration = configuration;
            _redisSerializer = redisSerializer;
            _connectionMultiplexer = new Lazy<ConnectionMultiplexer>(CreateConnectionMultiplexer);
        }

        /// <summary>
        ///     Gets the client.
        /// </summary>
        /// <returns></returns>
        public ICacheClient GetClient()
        {
            return new StackExchangeRedisCacheClient(_connectionMultiplexer.Value, _redisSerializer, _configuration.Configuration.Database);
        }

        /// <summary>
        ///     Creates the connection multiplexer.
        /// </summary>
        /// <returns></returns>
        private ConnectionMultiplexer CreateConnectionMultiplexer()
        {
            return ConnectionMultiplexer.Connect(_configuration.ConfigurationOptions);
        }
    }
}
