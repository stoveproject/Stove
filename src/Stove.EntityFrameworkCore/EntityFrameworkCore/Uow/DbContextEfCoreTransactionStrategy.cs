using System.Collections.Generic;

using Autofac.Extras.IocManager;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

using Stove.Collections.Extensions;
using Stove.Domain.Uow;
using Stove.EntityFrameworkCore.Extensions;
using System.Transactions;
using Stove.Transactions.Extensions;

namespace Stove.EntityFrameworkCore.Uow
{
	public class DbContextEfCoreTransactionStrategy : IEfCoreTransactionStrategy, ITransientDependency
	{
		public DbContextEfCoreTransactionStrategy()
		{
			ActiveTransactions = new Dictionary<string, ActiveTransactionInfo>();
		}

		protected UnitOfWorkOptions Options { get; private set; }

		protected IDictionary<string, ActiveTransactionInfo> ActiveTransactions { get; }

		public void InitOptions(UnitOfWorkOptions options)
		{
			Options = options;
		}

		public DbContext CreateDbContext<TDbContext>(string connectionString, IDbContextResolver dbContextResolver) where TDbContext : DbContext
		{
			DbContext dbContext;

			ActiveTransactionInfo activeTransaction = ActiveTransactions.GetOrDefault(connectionString);
			if (activeTransaction == null)
			{
				dbContext = dbContextResolver.Resolve<TDbContext>(connectionString, null);

				IDbContextTransaction dbtransaction = dbContext.Database.BeginTransaction((Options.IsolationLevel ?? IsolationLevel.ReadUncommitted).ToSystemDataIsolationLevel());
				activeTransaction = new ActiveTransactionInfo(dbtransaction, dbContext);
				ActiveTransactions[connectionString] = activeTransaction;
			}
			else
			{
				dbContext = dbContextResolver.Resolve<TDbContext>(
					connectionString,
					activeTransaction.DbContextTransaction.GetDbTransaction().Connection
				);

				if (dbContext.HasRelationalTransactionManager())
				{
					dbContext.Database.UseTransaction(activeTransaction.DbContextTransaction.GetDbTransaction());
				}
				else
				{
					dbContext.Database.BeginTransaction();
				}

				activeTransaction.AttendedDbContexts.Add(dbContext);
			}

			return dbContext;
		}

		public void Commit()
		{
			foreach (ActiveTransactionInfo activeTransaction in ActiveTransactions.Values)
			{
				activeTransaction.DbContextTransaction.Commit();

				foreach (DbContext dbContext in activeTransaction.AttendedDbContexts)
				{
					if (dbContext.HasRelationalTransactionManager())
					{
						continue; //Relational databases use the shared transaction
					}

					dbContext.Database.CommitTransaction();
				}
			}
		}

		public void Dispose()
		{
			foreach (ActiveTransactionInfo activeTransaction in ActiveTransactions.Values)
			{
				activeTransaction.DbContextTransaction.Dispose();

				foreach (DbContext attendedDbContext in activeTransaction.AttendedDbContexts)
				{
					//iocResolver.Release(attendedDbContext);
				}

				//iocResolver.Release(activeTransaction.StarterDbContext);
			}

			ActiveTransactions.Clear();
		}
	}
}
