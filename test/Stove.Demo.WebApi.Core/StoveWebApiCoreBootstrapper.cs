using Stove.Bootstrapping;
using Stove.Demo.WebApi.Core.Domain.DbContexts;

namespace Stove.Demo.WebApi.Core
{
	[DependsOn(
		typeof(StoveEntityFrameworkCoreBootstrapper),
		typeof(StoveRedisBootstrapper),
		typeof(StoveDapperBootstrapper),
		typeof(StoveMapsterBootstrapper),
		typeof(StoveHangFireBootstrapper),
		typeof(StoveRabbitMQBootstrapper)
	)]
	public class StoveWebApiCoreBootstrapper : StoveBootstrapper
	{
		public override void PreStart()
		{
			StoveConfiguration.DefaultNameOrConnectionString = "Default";
			StoveConfiguration.TypedConnectionStrings[typeof(AnimalDbContext)] = "Default";
			StoveConfiguration.TypedConnectionStrings[typeof(PersonDbContext)] = "Default";
		}
	}
}
