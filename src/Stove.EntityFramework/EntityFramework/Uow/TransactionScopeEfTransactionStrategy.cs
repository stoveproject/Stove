using System.Collections.Generic;
using System.Data.Entity;
using System.Transactions;

using Stove.Domain.Uow;

namespace Stove.EntityFramework.Uow
{
    public class TransactionScopeEfTransactionStrategy : IEfTransactionStrategy
    {
        public TransactionScopeEfTransactionStrategy()
        {
            DbContexts = new List<DbContext>();
        }

        protected UnitOfWorkOptions Options { get; private set; }

        protected TransactionScope CurrentTransaction { get; set; }

        protected List<DbContext> DbContexts { get; }

        public virtual void InitOptions(UnitOfWorkOptions options)
        {
            Options = options;
        }

        public virtual void Commit()
        {
            if (CurrentTransaction == null)
            {
                return;
            }

            CurrentTransaction.Complete();

            CurrentTransaction.Dispose();
            CurrentTransaction = null;
        }

        public virtual void Dispose()
        {
            DbContexts.Clear();

            if (CurrentTransaction != null)
            {
                CurrentTransaction.Dispose();
                CurrentTransaction = null;
            }
        }

        public DbContext CreateDbContext<TDbContext>(string connectionString, IDbContextResolver dbContextResolver) where TDbContext : DbContext
        {
            EnsureCurrentTransactionInitialized();

            var dbContext = dbContextResolver.Resolve<TDbContext>(connectionString);
            DbContexts.Add(dbContext);
            return dbContext;
        }

        private void EnsureCurrentTransactionInitialized()
        {
            if (CurrentTransaction != null)
            {
                return;
            }

            var transactionOptions = new TransactionOptions
            {
                IsolationLevel = Options.IsolationLevel.GetValueOrDefault(IsolationLevel.ReadUncommitted)
            };

            if (Options.Timeout.HasValue)
            {
                transactionOptions.Timeout = Options.Timeout.Value;
            }

            //TODO: LAZY!
            CurrentTransaction = new TransactionScope(
                Options.Scope.GetValueOrDefault(TransactionScopeOption.Required),
                transactionOptions,
                Options.AsyncFlowOption.GetValueOrDefault(TransactionScopeAsyncFlowOption.Enabled)
            );
        }
    }
}
