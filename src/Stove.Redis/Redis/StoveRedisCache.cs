using System;
using System.Reflection;
using System.Threading.Tasks;

using JetBrains.Annotations;

using StackExchange.Redis;

using Stove.Domain.Entities;
using Stove.Reflection.Extensions;
using Stove.Runtime.Caching;

namespace Stove.Redis
{
	/// <summary>
	///     Used to store cache in a Redis server.
	/// </summary>
	public class StoveRedisCache : CacheBase
	{
		private readonly IDatabase _database;
		private readonly IStoveRedisCacheDatabaseProvider _redisCacheDatabaseProvider;
		private readonly IRedisCacheSerializer _serializer;

		/// <summary>
		///     Constructor.
		/// </summary>
		public StoveRedisCache(
			[NotNull] string name,
			IRedisCacheSerializer serializer,
			IStoveRedisCacheDatabaseProvider redisCacheDatabaseProvider) : base(name)
		{
			_serializer = serializer;
			_redisCacheDatabaseProvider = redisCacheDatabaseProvider;
			_database = _redisCacheDatabaseProvider.GetDatabase();
		}

		public override object GetOrDefault(string key)
		{
			RedisValue objbyte = _database.StringGet(GetLocalizedKey(key));
			return objbyte.HasValue ? Deserialize(objbyte) : null;
		}

		public override void Set(string key, object value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null)
		{
			if (value == null)
			{
				throw new StoveException("Can not insert null values to the cache!");
			}

			Type type = value.GetType();
			if (EntityHelper.IsEntity(type) && type.GetAssembly().FullName.Contains("EntityFrameworkDynamicProxies"))
			{
				type = type.GetTypeInfo().BaseType;
			}

			_database.StringSet(
				GetLocalizedKey(key),
				Serialize(value, type),
				absoluteExpireTime ?? slidingExpireTime ?? DefaultAbsoluteExpireTime ?? DefaultSlidingExpireTime
			);
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

		[NotNull]
		protected virtual string GetLocalizedKey([NotNull] string key)
		{
			return "n:" + Name + ",c:" + key;
		}

		protected virtual object Deserialize(RedisValue objbyte)
		{
			return _serializer.Deserialize(objbyte);
		}

		protected virtual string Serialize(object value, Type type)
		{
			return _serializer.Serialize(value, type);
		}
	}
}
