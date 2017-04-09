using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Autofac.Extras.IocManager;

using Stove.Domain.Entities;

namespace Stove.RavenDB.Filters.Query
{
    public class RavenQueryFilterExecuter : IRavenQueryFilterExecuter, ITransientDependency
    {
        private readonly IEnumerable<IRavenQueryFilter> _filters;

        public RavenQueryFilterExecuter(IEnumerable<IRavenQueryFilter> filters)
        {
            _filters = filters;
        }

        public IQueryable<TEntity> ExecuteFilter<TEntity, TPrimaryKey>(IQueryable<TEntity> queryable) where TEntity : class, IEntity<TPrimaryKey>
        {
            _filters.ToList()
                    .ForEach(filter =>
                    {
                        Expression<Func<TEntity, bool>> predicate = filter.ExecuteFilter<TEntity, TPrimaryKey>();
                        if (predicate != null)
                        {
                            queryable = queryable.Where(predicate);
                        }
                    });

            return queryable;
        }
    }
}
