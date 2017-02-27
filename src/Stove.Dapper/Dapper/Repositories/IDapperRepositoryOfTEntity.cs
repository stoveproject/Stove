using Stove.Domain.Entities;

namespace Stove.Dapper.Dapper.Repositories
{
    public interface IDapperRepository<TEntity> : IDapperRepository<TEntity, int> where TEntity : class, IEntity<int>
    {
    }
}
