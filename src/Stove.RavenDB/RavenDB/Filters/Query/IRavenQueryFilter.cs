using System;
using System.Linq.Expressions;

using Stove.Domain.Entities;

namespace Stove.RavenDB.Filters.Query
{
    public interface IRavenQueryFilter
    {
        string FilterName { get; }

        bool IsEnabled { get; }

        Expression<Func<TEntity, bool>> ExecuteFilter<TEntity, TPrimaryKey>() where TEntity : class, IEntity<TPrimaryKey>;
    }
}
