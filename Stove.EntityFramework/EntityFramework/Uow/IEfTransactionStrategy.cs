using System.Data.Entity;

using Autofac.Extras.IocManager;

using Stove.Domain.Uow;

namespace Stove.EntityFramework.EntityFramework.Uow
{
    public interface IEfTransactionStrategy
    {
        void InitOptions(UnitOfWorkOptions options);

        void Commit();

        void InitDbContext(DbContext dbContext, string connectionString);

        void Dispose(IScopeResolver iocResolver);
    }
}
