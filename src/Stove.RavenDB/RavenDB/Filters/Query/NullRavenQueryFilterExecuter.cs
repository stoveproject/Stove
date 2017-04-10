using System.Linq;

using Stove.Domain.Entities;

namespace Stove.RavenDB.Filters.Query
{
    public class NullRavenQueryFilterExecuter : IRavenQueryFilterExecuter
    {
        public static readonly NullRavenQueryFilterExecuter Instance = new NullRavenQueryFilterExecuter();

        public IQueryable<TEntity> ExecuteFilter<TEntity, TPrimaryKey>(IQueryable<TEntity> queryable) where TEntity : class, IEntity<TPrimaryKey>
        {
            return queryable;
        }
    }
}
