using System;

using Autofac.Extras.IocManager;

using JetBrains.Annotations;

using StackExchange.Redis;

using Stove.Redis.Configurations;

namespace Stove.Redis
{
	/// <summary>
	///     Implements <see cref="IStoveRedisCacheDatabaseProvider" />.
	/// </summary>
	public class StoveRedisCacheDatabaseProvider : IStoveRedisCacheDatabaseProvider, ISingletonDependency
	{
		private readonly IStoveRedisCacheConfiguration _configuration;
		private readonly Lazy<ConnectionMultiplexer> _connectionMultiplexer;

		/// <summary>
		///     Initializes a new instance of the <see cref="StoveRedisCacheDatabaseProvider" /> class.
		/// </summary>
		public StoveRedisCacheDatabaseProvider([NotNull] IStoveRedisCacheConfiguration configuration)
		{
			_configuration = configuration;
			_connectionMultiplexer = new Lazy<ConnectionMultiplexer>(CreateConnectionMultiplexer);
		}

		/// <summary>
		///     Gets the client.
		/// </summary>
		/// <returns></returns>
		public IDatabase GetDatabase()
		{
			return _connectionMultiplexer.Value.GetDatabase();
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
