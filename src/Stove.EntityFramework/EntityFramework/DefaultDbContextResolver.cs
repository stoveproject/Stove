using System;

using Autofac.Extras.IocManager;

namespace Stove.EntityFramework.EntityFramework
{
    public class DefaultDbContextResolver : IDbContextResolver, ITransientDependency
    {
        private readonly IDbContextTypeMatcher _dbContextTypeMatcher;
        private readonly IScopeResolver _scopedResolver;

        public DefaultDbContextResolver(IScopeResolver iocResolver, IDbContextTypeMatcher dbContextTypeMatcher)
        {
            _scopedResolver = iocResolver;
            _dbContextTypeMatcher = dbContextTypeMatcher;
        }

        public TDbContext Resolve<TDbContext>(string connectionString)
        {
            Type dbContextType = typeof(TDbContext);

            if (!dbContextType.IsAbstract)
            {
                return _scopedResolver.Resolve<TDbContext>(new
                {
                    nameOrConnectionString = connectionString
                });
            }

            Type concreteType = _dbContextTypeMatcher.GetConcreteType(dbContextType);
            return (TDbContext)_scopedResolver.Resolve(concreteType, new
            {
                nameOrConnectionString = connectionString
            });
        }
    }
}
