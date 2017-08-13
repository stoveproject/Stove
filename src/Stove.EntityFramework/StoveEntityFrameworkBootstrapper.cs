using System;
using System.Linq;

using Stove.Bootstrapping;
using Stove.EntityFramework;
using Stove.EntityFramework.Common;

namespace Stove
{
	[DependsOn(
		typeof(StoveKernelBootstrapper)
	)]
	public class StoveEntityFrameworkBootstrapper : StoveBootstrapper
	{
		private readonly IDbContextTypeMatcher _dbContextTypeMatcher;

		public StoveEntityFrameworkBootstrapper(IDbContextTypeMatcher dbContextTypeMatcher)
		{
			_dbContextTypeMatcher = dbContextTypeMatcher;
		}

		public override void Start()
		{
			Type[] dbContextTypes = Resolver.GetRegisteredServices().Where(x => typeof(StoveDbContext).IsAssignableFrom(x)).ToArray();
			_dbContextTypeMatcher.Populate(dbContextTypes);
		}
	}
}
