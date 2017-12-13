using Stove.Data;
using Stove.Domain.Entities;

namespace Stove.Dapper.Repositories
{
    public class DapperNhRepositoryBase<TStoveSessionContext, TEntity> : DapperNhRepositoryBase<TStoveSessionContext, TEntity, int>, IDapperRepository<TEntity>
        where TEntity : class, IEntity<int>
        where TStoveSessionContext : class
    {
        public DapperNhRepositoryBase(IActiveTransactionProvider activeTransactionProvider) : base(activeTransactionProvider)
        {
        }
    }
}
