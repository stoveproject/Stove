using System;
using System.Data.Common;

using Autofac.Extras.IocManager;

namespace Stove.EntityFramework.EntityFramework
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
            Type dbContextType = typeof(TDbContext);

            if (!dbContextType.IsAbstract)
            {
                return _scopeResolver.Resolve<TDbContext>(new
                {
                    nameOrConnectionString = connectionString
                });
            }

            Type concreteType = _dbContextTypeMatcher.GetConcreteType(dbContextType);
            return (TDbContext)_scopeResolver.Resolve(concreteType, new
            {
                nameOrConnectionString = connectionString
            });
        }

        public TDbContext Resolve<TDbContext>(DbConnection existingConnection)
        {
            Type dbContextType = typeof(TDbContext);

            if (!dbContextType.IsAbstract)
            {
                return _scopeResolver.Resolve<TDbContext>(new
                {
                    existingConnection
                });
            }

            Type concreteType = _dbContextTypeMatcher.GetConcreteType(dbContextType);
            return (TDbContext)_scopeResolver.Resolve(concreteType, new
            {
                existingConnection
            });
        }
    }
}
