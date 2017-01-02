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
            string activeTransactionKey = $"{dbContext.GetType().FullName}#{connectionString}";
            ActiveTransactionInfo activeTransaction = ActiveTransactions.GetOrDefault(activeTransactionKey);
            if (activeTransaction == null)
            {
                activeTransaction = new ActiveTransactionInfo(
                    dbContext.Database.BeginTransaction(
                        (Options.IsolationLevel ?? IsolationLevel.ReadUncommitted).ToSystemDataIsolationLevel()
                    ),
                    dbContext
                );

                ActiveTransactions[activeTransactionKey] = activeTransaction;
            }
            else
            {
                dbContext.Database.UseTransaction(activeTransaction.DbContextTransaction.UnderlyingTransaction);
                activeTransaction.AttendedDbContexts.Add(dbContext);
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
    }
}
