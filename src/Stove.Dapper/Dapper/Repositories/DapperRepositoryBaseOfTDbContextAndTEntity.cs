using Stove.Domain.Entities;
using Stove.Orm;

namespace Stove.Dapper.Repositories
{
    public sealed class DapperRepositoryIntBase<TDbContext, TEntity> : DapperRepositoryBase<TDbContext, TEntity, int>, IDapperRepository<TEntity>
        where TEntity : class, IEntity<int>

    {
        public DapperRepositoryIntBase(IActiveTransactionOrConnectionProvider activeTransactionOrConnectionProvider) : base(activeTransactionOrConnectionProvider)
        {
        }
    }
}
