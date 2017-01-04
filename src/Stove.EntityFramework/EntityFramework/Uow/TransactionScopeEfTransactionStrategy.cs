using System.Collections.Generic;
using System.Data.Entity;
using System.Transactions;

using Stove.Domain.Uow;

namespace Stove.EntityFramework.EntityFramework.Uow
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
            CurrentTransaction?.Complete();
        }

        public virtual void InitDbContext(DbContext dbContext, string connectionString)
        {
            EnsureCurrentTransactionInitialized();
            DbContexts.Add(dbContext);
        }

        public virtual void Dispose()
        {
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

        /// <summary>
        ///     Gets active transaction for beginned transactions, returns null when TransactionScopeEFTransactionStrategy is
        ///     active.
        /// </summary>
        /// <typeparam name="TDbContext">The type of the database context.</typeparam>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        public ActiveTransactionInfo GetActiveTransaction<TDbContext>(string connectionString) where TDbContext : DbContext
        {
            return null;
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
