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
			Configuration.DefaultNameOrConnectionString = "Default";
			Configuration.UnitOfWork.IsolationLevel = IsolationLevel.Unspecified;
		}
	}
}
