using Stove.Domain.Entities;

namespace Stove.Domain.Repositories
{
    public interface IDapperRepository<TEntity> : IDapperRepository<TEntity, int> where TEntity : class, IEntity<int>
    {
    }
}
