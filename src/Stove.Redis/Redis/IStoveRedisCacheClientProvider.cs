using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core;

namespace Stove.Redis.Redis
{
    /// <summary>
    ///     Used to get <see cref="IDatabase" /> for Redis cache.
    /// </summary>
    public interface IStoveRedisCacheClientProvider
    {
        /// <summary>
        ///     Gets the client.
        /// </summary>
        /// <returns></returns>
        ICacheClient GetClient();
    }
}
