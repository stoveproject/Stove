using Stove.Domain.Entities;

namespace Stove.Dapper.Filters.Action
{
    public interface IDapperActionFilter
    {
        void ExecuteFilter<TEntity, TPrimaryKey>(TEntity entity) where TEntity : class, IEntity<TPrimaryKey>;
    }
}
