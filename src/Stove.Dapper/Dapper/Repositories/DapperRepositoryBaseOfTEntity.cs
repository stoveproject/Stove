using System.Data.Entity;

using Stove.Domain.Entities;
using Stove.EntityFramework.EntityFramework;

namespace Stove.Dapper.Dapper.Repositories
{
    public class DapperRepositoryBase<TDbContext, TEntity> : DapperRepositoryBase<TDbContext, TEntity, int>, IDapperRepository<TEntity>
        where TEntity : class, IEntity<int>
        where TDbContext : DbContext
    {
        public DapperRepositoryBase(IDbContextProvider<TDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
