using System.Data;

using Stove.Domain.Entities;
using Stove.Orm;

namespace Stove.Dapper.Repositories
{
    public class DapperRepositoryBase<TDbContext, TEntity, TPrimaryKey> : DapperRepositoryBase<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>

    {
        private readonly IActiveTransactionProvider _activeTransactionProvider;

        public DapperRepositoryBase(IActiveTransactionProvider activeTransactionProvider) : base(activeTransactionProvider)
        {
            _activeTransactionProvider = activeTransactionProvider;
        }

        public override IDbConnection Connection
        {
            get { return _activeTransactionProvider.GetActiveConnection(typeof(TDbContext)); }
        }

        public override IDbTransaction ActiveTransaction
        {
            get { return _activeTransactionProvider.GetActiveTransaction(typeof(TDbContext)); }
        }
    }
}
