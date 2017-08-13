using Autofac.Extras.IocManager;

using Microsoft.EntityFrameworkCore;

using Stove.Domain.Uow;

namespace Stove.EntityFrameworkCore.Uow
{
	public interface IEfCoreTransactionStrategy
	{
		void InitOptions(UnitOfWorkOptions options);

		DbContext CreateDbContext<TDbContext>(string connectionString, IDbContextResolver dbContextResolver)
			where TDbContext : DbContext;

		void Commit();

		void Dispose();
	}
}
