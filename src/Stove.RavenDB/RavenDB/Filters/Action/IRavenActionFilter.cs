using Stove.Domain.Entities;

namespace Stove.RavenDB.Filters.Action
{
    public interface IRavenActionFilter
    {
        void ExecuteFilter<TEntity, TPrimaryKey>(TEntity entity) where TEntity : class, IEntity<TPrimaryKey>;
    }
}
