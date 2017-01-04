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

            ActiveTransactionInfo activeTransaction = ActiveTransactions.GetOrDefault(connectionString);
            if (activeTransaction == null)
            {
                dbContext = dbContextResolver.Resolve<TDbContext>(connectionString);
                DbContextTransaction dbtransaction = dbContext.Database.BeginTransaction((Options.IsolationLevel ?? IsolationLevel.ReadUncommitted).ToSystemDataIsolationLevel());
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
