using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using DapperExtensions;

using Stove.Domain.Entities;

namespace Stove.Dapper.Dapper
{
    public interface IDapperQueryFilter
    {
        string FilterName { get; }

        bool IsEnabled { get;  }

        IFieldPredicate ExecuteFilter<TEntity, TPrimaryKey>() where TEntity : class, IEntity<TPrimaryKey>;

        IList<TEntity> ExecuteFilter<TEntity, TPrimaryKey>(IList<TEntity> source) where TEntity : class, IEntity<TPrimaryKey>;

        Expression<Func<TEntity, bool>> ExecuteFilter<TEntity, TPrimaryKey>(Expression<Func<TEntity, bool>> predicate) where TEntity : class, IEntity<TPrimaryKey>;
    }
}
