using System;
using System.Linq;
using System.Reflection;

using Stove.Bootstrapping;
using Stove.EntityFramework;
using Stove.EntityFrameworkCore;
using Stove.EntityFrameworkCore.Configuration;

namespace Stove
{
	public class StoveEntityFrameworkCoreBootstrapper : StoveBootstrapper
	{
		private readonly IDbContextTypeMatcher _dbContextTypeMatcher;

		public StoveEntityFrameworkCoreBootstrapper(IDbContextTypeMatcher dbContextTypeMatcher)
		{
			_dbContextTypeMatcher = dbContextTypeMatcher;
		}

		public override void PreStart()
		{
			StoveConfiguration.GetConfigurerIfExists<IStoveEfCoreConfiguration>()(StoveConfiguration.Modules.StoveEfCore());
		}

		public override void Start()
		{
			Type[] dbContextTypes = Resolver.GetRegisteredServices().Where(x => typeof(StoveDbContext).GetTypeInfo().IsAssignableFrom(x)).ToArray();
			_dbContextTypeMatcher.Populate(dbContextTypes);
		}
	}
}
