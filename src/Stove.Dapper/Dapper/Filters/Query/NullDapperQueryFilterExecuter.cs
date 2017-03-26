using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

using DapperExtensions;

using Stove.Domain.Entities;

namespace Stove.Dapper.Filters.Query
{
    [ExcludeFromCodeCoverage]
    public class NullDapperQueryFilterExecuter : IDapperQueryFilterExecuter
    {
        public static readonly NullDapperQueryFilterExecuter Instance = new NullDapperQueryFilterExecuter();

        public IPredicate ExecuteFilter<TEntity, TPrimaryKey>(Expression<Func<TEntity, bool>> predicate) where TEntity : class, IEntity<TPrimaryKey>
        {
            return null;
        }

        public PredicateGroup ExecuteFilter<TEntity, TPrimaryKey>() where TEntity : class, IEntity<TPrimaryKey>
        {
            return null;
        }
    }
}
