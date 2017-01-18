using System;

using Autofac.Extras.IocManager;

using StackExchange.Redis;

namespace Stove.Redis.Redis
{
    /// <summary>
    ///     Implements <see cref="IStoveRedisCacheDatabaseProvider" />.
    /// </summary>
    public class StoveRedisCacheDatabaseProvider : IStoveRedisCacheDatabaseProvider, ISingletonDependency
    {
        private readonly Lazy<ConnectionMultiplexer> _connectionMultiplexer;
        private readonly StoveRedisCacheOptions _options;

        /// <summary>
        ///     Initializes a new instance of the <see cref="StoveRedisCacheDatabaseProvider" /> class.
        /// </summary>
        public StoveRedisCacheDatabaseProvider(StoveRedisCacheOptions options)
        {
            _options = options;
            _connectionMultiplexer = new Lazy<ConnectionMultiplexer>(CreateConnectionMultiplexer);
        }

        /// <summary>
        ///     Gets the database connection.
        /// </summary>
        public IDatabase GetDatabase()
        {
            return _connectionMultiplexer.Value.GetDatabase(_options.DatabaseId);
        }

        private ConnectionMultiplexer CreateConnectionMultiplexer()
        {
            return ConnectionMultiplexer.Connect(_options.ConnectionString);
        }
    }
}
