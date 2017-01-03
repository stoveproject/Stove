using System.Data.Entity;

using Stove.Domain.Uow;

namespace Stove.EntityFramework.EntityFramework.Uow
{
    public interface IEfTransactionStrategy
    {
        void InitOptions(UnitOfWorkOptions options);

        void Commit();

        void InitDbContext(DbContext dbContext, string connectionString);

        void Dispose();

        ActiveTransactionInfo GetActiveTransaction<TDbContext>(string connectionString) where TDbContext : DbContext;
    }
}
