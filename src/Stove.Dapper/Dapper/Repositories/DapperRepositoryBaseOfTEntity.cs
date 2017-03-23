using System.Data.Entity;

using JetBrains.Annotations;

using Stove.Domain.Entities;
using Stove.Orm;

namespace Stove.Dapper.Repositories
{
    public class DapperRepositoryBase<TDbContext, TEntity> : DapperRepositoryBase<TDbContext, TEntity, int>, IDapperRepository<TEntity>
        where TEntity : class, IEntity<int>
        where TDbContext : DbContext
    {
        public DapperRepositoryBase([NotNull] IActiveTransactionProvider activeTransactionProvider) : base(activeTransactionProvider)
        {
        }
    }
}
