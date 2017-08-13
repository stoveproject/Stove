using Microsoft.EntityFrameworkCore;

using Stove.Domain.Entities;
using Stove.Domain.Repositories;

namespace Stove.EntityFrameworkCore.Repositories
{
	public static class EfCoreRepositoryExtensions
	{
		public static DbContext GetDbContext<TEntity, TPrimaryKey>(this IRepository<TEntity, TPrimaryKey> repository)
			where TEntity : class, IEntity<TPrimaryKey>
		{
			return (repository as IRepositoryWithDbContext).GetDbContext();
		}

		public static void DetachFromDbContext<TEntity, TPrimaryKey>(this IRepository<TEntity, TPrimaryKey> repository, TEntity entity)
			where TEntity : class, IEntity<TPrimaryKey>
		{
			repository.GetDbContext().Entry(entity).State = EntityState.Detached;
		}
	}
}
