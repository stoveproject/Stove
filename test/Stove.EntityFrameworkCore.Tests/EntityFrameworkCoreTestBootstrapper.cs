using System.Transactions;

using Stove.Bootstrapping;

namespace Stove.EntityFrameworkCore.Tests
{
	[DependsOn(
		typeof(StoveEntityFrameworkCoreBootstrapper)
	)]
	public class EntityFrameworkCoreTestBootstrapper : StoveBootstrapper
	{
		public override void PreStart()
		{
			StoveConfiguration.DefaultNameOrConnectionString = "Default";
			StoveConfiguration.UnitOfWork.IsolationLevel = IsolationLevel.Unspecified;
		}
	}
}
