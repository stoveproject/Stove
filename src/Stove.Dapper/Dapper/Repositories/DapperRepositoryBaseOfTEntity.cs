using Stove.Domain.Entities;
using Stove.Orm;

namespace Stove.Dapper.Repositories
{
    public class DapperRepositoryBase<TEntity> : DapperRepositoryBase<TEntity, int>, IDapperRepository<TEntity> where TEntity : class, IEntity<int>
    {
        public DapperRepositoryBase(IActiveTransactionProvider activeTransactionProvider) : base(activeTransactionProvider)
        {
        }
    }
}
