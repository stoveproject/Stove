using JetBrains.Annotations;

using StackExchange.Redis;

namespace Stove.Redis
{
	/// <summary>
	///     Used to get <see cref="IDatabase" /> for Redis cache.
	/// </summary>
	public interface IStoveRedisCacheDatabaseProvider
	{
		/// <summary>
		///     Gets the client.
		/// </summary>
		/// <returns></returns>
		[NotNull]
		IDatabase GetDatabase();
	}
}
