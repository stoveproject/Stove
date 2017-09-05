using JetBrains.Annotations;

using StackExchange.Redis;

using Stove.Configuration;

namespace Stove.Redis.Configurations
{
	public interface IStoveRedisCacheConfiguration
	{
		[CanBeNull]
		ConfigurationOptions ConfigurationOptions { get; set; }

		IStoveStartupConfiguration StoveConfiguration { get; }
	}
}
