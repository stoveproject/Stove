using System.Linq;

using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;

using Stove.Domain.Entities;
using Stove.Domain.Repositories;
using Stove.Events.Bus.Entities;
using Stove.RavenDB.Filters.Action;
using Stove.RavenDB.Filters.Query;

namespace Stove.RavenDB.Repositories
{
	public class RavenDBRepositoryBase<TEntity, TPrimaryKey> : StoveRepositoryBase<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
	{
		private readonly ISessionProvider _sessionProvider;

		public RavenDBRepositoryBase(ISessionProvider sessionProvider)
		{
			_sessionProvider = sessionProvider;

			EntityChangeEventHelper = NullEntityChangeEventHelper.Instance;
			RavenQueryFilterExecuter = NullRavenQueryFilterExecuter.Instance;
			RavenActionFilterExecuter = NullRavenActionFilterExecuter.Instance;
		}

		public IDocumentSession Session => _sessionProvider.Session;

		public IEntityChangeEventHelper EntityChangeEventHelper { get; set; }

		public IRavenQueryFilterExecuter RavenQueryFilterExecuter { get; set; }

		public IRavenActionFilterExecuter RavenActionFilterExecuter { get; set; }

		/// <summary>
		///     Gets all.
		/// </summary>
		/// <returns></returns>
		public override IQueryable<TEntity> GetAll()
		{
			IRavenQueryable<TEntity> queryable = Session.Query<TEntity>();
			return RavenQueryFilterExecuter.ExecuteFilter<TEntity, TPrimaryKey>(queryable);
		}

		/// <summary>
		///     Inserts the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns></returns>
		public override TEntity Insert(TEntity entity)
		{
			RavenActionFilterExecuter.ExecuteCreationAuditFilter<TEntity, TPrimaryKey>(entity);
			Session.Store(entity);
			return entity;
		}

		/// <summary>
		///     Updates the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns></returns>
		public override TEntity Update(TEntity entity)
		{
			RavenActionFilterExecuter.ExecuteModificationAuditFilter<TEntity, TPrimaryKey>(entity);
			Session.Store(entity);
			return entity;
		}

		/// <summary>
		///     Deletes the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public override void Delete(TEntity entity)
		{
			if (entity is ISoftDelete)
			{
				RavenActionFilterExecuter.ExecuteDeletionAuditFilter<TEntity, TPrimaryKey>(entity);
				Session.Store(entity);
			}
			else
			{
				Session.Delete(entity);
			}
		}

		/// <summary>
		///     Deletes the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		public override void Delete(TPrimaryKey id)
		{
			TEntity entity = Get(id);
			Delete(entity);
		}
	}
}
