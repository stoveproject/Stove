using StackExchange.Redis;

using Stove.Configuration;

namespace Stove.Redis.Configurations
{
	public class StoveRedisCacheConfiguration : IStoveRedisCacheConfiguration
	{
		public StoveRedisCacheConfiguration(IStoveStartupConfiguration configuration)
		{
			StoveConfiguration = configuration;
		}

		public ConfigurationOptions ConfigurationOptions { get; set; }

		public IStoveStartupConfiguration StoveConfiguration { get; }
	}
}
