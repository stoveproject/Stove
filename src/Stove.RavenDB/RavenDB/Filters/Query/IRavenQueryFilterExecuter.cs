using System.Linq;

using Stove.Domain.Entities;

namespace Stove.RavenDB.Filters.Query
{
    public interface IRavenQueryFilterExecuter
    {
        IQueryable<TEntity> ExecuteFilter<TEntity, TPrimaryKey>(IQueryable<TEntity> queryable) where TEntity : class, IEntity<TPrimaryKey>;
    }
}
