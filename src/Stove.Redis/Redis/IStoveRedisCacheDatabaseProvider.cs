using StackExchange.Redis;

namespace Stove.Redis.Redis
{
    /// <summary>
    ///     Used to get <see cref="IDatabase" /> for Redis cache.
    /// </summary>
    public interface IStoveRedisCacheDatabaseProvider
    {
        /// <summary>
        ///     Gets the database connection.
        /// </summary>
        IDatabase GetDatabase();
    }
}
