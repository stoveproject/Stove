using Stove.Bootstrapping;
using Stove.Redis.Configurations;

namespace Stove
{
	[DependsOn(
		typeof(StoveKernelBootstrapper)
	)]
	public class StoveRedisBootstrapper : StoveBootstrapper
	{
		public override void PreStart()
		{
			if (StoveConfiguration.IsConfigurerRegistered<IStoveRedisCacheConfiguration>())
			{
				StoveConfiguration.GetConfigurerIfExists<IStoveRedisCacheConfiguration>().Invoke(StoveConfiguration.Modules.StoveRedis());
			}
		}
	}
}
