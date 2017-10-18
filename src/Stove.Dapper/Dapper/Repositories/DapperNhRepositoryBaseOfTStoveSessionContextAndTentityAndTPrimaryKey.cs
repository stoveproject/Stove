using System.Data.Common;

using Stove.Data;
using Stove.Domain.Entities;

namespace Stove.Dapper.Repositories
{
    public class DapperNhRepositoryBase<TStoveSessionContext, TEntity, TPrimaryKey> : DapperRepositoryBase<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
        where TStoveSessionContext : class
    {
        private readonly IActiveTransactionProvider _activeTransactionProvider;

        public DapperNhRepositoryBase(IActiveTransactionProvider activeTransactionProvider) : base(activeTransactionProvider)
        {
            _activeTransactionProvider = activeTransactionProvider;
        }

        public ActiveTransactionProviderArgs ActiveTransactionProviderArgs => new ActiveTransactionProviderArgs
        {
            ["SessionContextType"] = typeof(TStoveSessionContext)
        };

        public override DbConnection Connection => (DbConnection)_activeTransactionProvider.GetActiveConnection(ActiveTransactionProviderArgs);

        public override DbTransaction ActiveTransaction => (DbTransaction)_activeTransactionProvider.GetActiveTransaction(ActiveTransactionProviderArgs);
    }
}
