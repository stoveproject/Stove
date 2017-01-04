using System.Collections.Generic;
using System.Data.Entity;
using System.Transactions;

using Stove.Collections.Extensions;
using Stove.Domain.Uow;
using Stove.Transactions.Extensions;

namespace Stove.EntityFramework.EntityFramework.Uow
{
    public class DbContextEfTransactionStrategy : IEfTransactionStrategy
    {
        public DbContextEfTransactionStrategy()
        {
            ActiveTransactions = new Dictionary<string, ActiveTransactionInfo>();
        }

        protected UnitOfWorkOptions Options { get; private set; }

        protected IDictionary<string, ActiveTransactionInfo> ActiveTransactions { get; }

        public void InitOptions(UnitOfWorkOptions options)
        {
            Options = options;
        }

        public void Commit()
        {
            foreach (ActiveTransactionInfo activeTransaction in ActiveTransactions.Values)
            {
                activeTransaction.DbContextTransaction.Commit();
            }
        }

        public void InitDbContext(DbContext dbContext, string connectionString)
        {
            ActiveTransactionInfo activeTransaction = ActiveTransactions.GetOrDefault(connectionString);
            if (activeTransaction == null)
            {
                DbContextTransaction dbtransaction = dbContext.Database.BeginTransaction((Options.IsolationLevel ?? IsolationLevel.ReadUncommitted).ToSystemDataIsolationLevel());
                activeTransaction = new ActiveTransactionInfo(dbtransaction, dbContext);
                ActiveTransactions[connectionString] = activeTransaction;
            }
            else
            {
                dbContext.Database.UseTransaction(activeTransaction.DbContextTransaction.UnderlyingTransaction);
                activeTransaction.AttendedDbContexts.Add(dbContext);
            }
        }

        public ActiveTransactionInfo GetActiveTransaction<TDbContext>(string connectionString) where TDbContext : DbContext
        {
            return ActiveTransactions.GetOrDefault(connectionString);
        }

        public void Dispose()
        {
            foreach (ActiveTransactionInfo activeTransaction in ActiveTransactions.Values)
            {
                activeTransaction.DbContextTransaction.Dispose();
                activeTransaction.StarterDbContext.Dispose();
            }

            ActiveTransactions.Clear();
        }

        public DbContext CreateDbContext<TDbContext>(string connectionString, IDbContextResolver dbContextResolver) where TDbContext : DbContext
        {
            DbContext dbContext;

            var activeTransaction = ActiveTransactions.GetOrDefault(connectionString);
            if (activeTransaction == null)
            {
                dbContext = dbContextResolver.Resolve<TDbContext>(connectionString);
                var dbtransaction = dbContext.Database.BeginTransaction((Options.IsolationLevel ?? IsolationLevel.ReadUncommitted).ToSystemDataIsolationLevel());
                activeTransaction = new ActiveTransactionInfo(dbtransaction, dbContext);
                ActiveTransactions[connectionString] = activeTransaction;
            }
            else
            {
                dbContext = dbContextResolver.Resolve<TDbContext>(activeTransaction.DbContextTransaction.UnderlyingTransaction.Connection, false);
                activeTransaction.AttendedDbContexts.Add(dbContext);
            }

            return dbContext;
        }
    }
}
