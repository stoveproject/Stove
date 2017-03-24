using System.Data;

using Stove.Domain.Entities;
using Stove.Orm;

namespace Stove.Dapper.Repositories
{
    public class DapperEfRepositoryBase<TDbContext, TEntity, TPrimaryKey> : DapperRepositoryBase<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>

    {
        private readonly IActiveTransactionOrConnectionProvider _activeTransactionOrConnectionProvider;

        public DapperEfRepositoryBase(IActiveTransactionOrConnectionProvider activeTransactionOrConnectionProvider) : base(activeTransactionOrConnectionProvider)
        {
            _activeTransactionOrConnectionProvider = activeTransactionOrConnectionProvider;
        }

        public ActiveTransactionOrConnectionProviderArgs ActiveTransactionOrConnectionProviderArgs
        {
            get
            {
                var args = new ActiveTransactionOrConnectionProviderArgs();
                args["ContextType"] = typeof(TDbContext);
                return args;
            }
        }

        public override IDbConnection Connection
        {
            get { return _activeTransactionOrConnectionProvider.GetActiveConnection(ActiveTransactionOrConnectionProviderArgs); }
        }

        public override IDbTransaction ActiveTransaction
        {
            get { return _activeTransactionOrConnectionProvider.GetActiveTransaction(ActiveTransactionOrConnectionProviderArgs); }
        }
    }
}
