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
			if (Configuration.IsConfigurerRegistered<IStoveRedisCacheConfiguration>())
			{
				Configuration.GetConfigurerIfExists<IStoveRedisCacheConfiguration>().Invoke(Configuration.Modules.StoveRedis());
			}
		}
	}
}
