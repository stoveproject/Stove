using System;
using System.Data.Common;

using Autofac.Extras.IocManager;

using Stove.EntityFramework.Common;

namespace Stove.EntityFramework
{
	public class DefaultDbContextResolver : IDbContextResolver, ITransientDependency
	{
		private readonly IDbContextTypeMatcher _dbContextTypeMatcher;
		private readonly IScopeResolver _scopeResolver;

		public DefaultDbContextResolver(IScopeResolver scopeResolver, IDbContextTypeMatcher dbContextTypeMatcher)
		{
			_scopeResolver = scopeResolver;
			_dbContextTypeMatcher = dbContextTypeMatcher;
		}

		public TDbContext Resolve<TDbContext>(string connectionString)
		{
			Type dbContextType = GetConcreteType<TDbContext>();
			return (TDbContext)_scopeResolver.Resolve(dbContextType, new
			{
				nameOrConnectionString = connectionString
			});
		}

		public TDbContext Resolve<TDbContext>(DbConnection existingConnection, bool contextOwnsConnection)
		{
			Type dbContextType = GetConcreteType<TDbContext>();
			return (TDbContext)_scopeResolver.Resolve(dbContextType, new
			{
				existingConnection,
				contextOwnsConnection
			});
		}

		protected virtual Type GetConcreteType<TDbContext>()
		{
			Type dbContextType = typeof(TDbContext);
			return !dbContextType.IsAbstract
				? dbContextType
				: _dbContextTypeMatcher.GetConcreteType(dbContextType);
		}
	}
}
