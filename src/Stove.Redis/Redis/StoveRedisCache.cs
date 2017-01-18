using System;
using System.Threading.Tasks;

using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core;

using Stove.Runtime.Caching;

namespace Stove.Redis.Redis
{
    /// <summary>
    ///     Used to store cache in a Redis server.
    /// </summary>
    public class StoveRedisCache : CacheBase
    {
        private readonly ICacheClient _cacheClient;
        private readonly IDatabase _database;

        /// <summary>
        ///     Constructor.
        /// </summary>
        public StoveRedisCache(string name, IStoveRedisCacheDatabaseProvider redisCacheDatabaseProvider) : base(name)
        {
            _cacheClient = redisCacheDatabaseProvider.GetClient();
            _database = _cacheClient.Database;
        }

        public override object GetOrDefault(string key)
        {
            return _cacheClient.Get<object>(GetLocalizedKey(key));
        }

        public override Task<object> GetOrDefaultAsync(string key)
        {
            return _cacheClient.GetAsync<object>(GetLocalizedKey(key));
        }

        public override void Set(string key, object value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null)
        {
            if (value == null)
            {
                throw new StoveException("Can not insert null values to the cache!");
            }
           
            _cacheClient.Add(GetLocalizedKey(key), value, absoluteExpireTime ?? slidingExpireTime ?? DefaultAbsoluteExpireTime ?? DefaultSlidingExpireTime);
        }

        public override Task SetAsync(string key, object value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null)
        {
            return _cacheClient.AddAsync(GetLocalizedKey(key), value, absoluteExpireTime ?? slidingExpireTime ?? DefaultAbsoluteExpireTime ?? DefaultSlidingExpireTime);
        }

        public override void Remove(string key)
        {
            _database.KeyDelete(GetLocalizedKey(key));
        }

        public override Task RemoveAsync(string key)
        {
            return _database.KeyDeleteAsync(GetLocalizedKey(key));
        }

        public override void Clear()
        {
            _database.KeyDeleteWithPrefix(GetLocalizedKey("*"));
        }

        protected virtual string GetLocalizedKey(string key)
        {
            return "n:" + Name + ",c:" + key;
        }
    }
}
