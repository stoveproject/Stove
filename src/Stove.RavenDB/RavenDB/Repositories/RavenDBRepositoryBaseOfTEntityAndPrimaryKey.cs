using System.Linq;

using Raven.Client;

using Stove.Domain.Entities;
using Stove.Domain.Repositories;
using Stove.Events.Bus.Entities;

namespace Stove.RavenDB.Repositories
{
    public class RavenDBRepositoryBase<TEntity, TPrimaryKey> : StoveRepositoryBase<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
    {
        public IDocumentSession Session => _sessionProvider.Session;
        private readonly ISessionProvider _sessionProvider;

        public RavenDBRepositoryBase(ISessionProvider sessionProvider)
        {
            _sessionProvider = sessionProvider;

            EntityChangeEventHelper = NullEntityChangeEventHelper.Instance;
        }
        
        public IEntityChangeEventHelper EntityChangeEventHelper { get; set; }

        /// <summary>
        ///     Gets all.
        /// </summary>
        /// <returns></returns>
        public override IQueryable<TEntity> GetAll()
        {
            return Session.Query<TEntity>();
        }

        /// <summary>
        ///     Inserts the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public override TEntity Insert(TEntity entity)
        {
            EntityChangeEventHelper.TriggerEntityCreatingEvent(entity);
            Session.Store(entity);
            EntityChangeEventHelper.TriggerEntityCreatedEventOnUowCompleted(entity);
            return entity;
        }

        /// <summary>
        ///     Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public override TEntity Update(TEntity entity)
        {
            EntityChangeEventHelper.TriggerEntityUpdatingEvent(entity);
            Session.Store(entity);
            EntityChangeEventHelper.TriggerEntityUpdatedEventOnUowCompleted(entity);
            return entity;
        }

        /// <summary>
        ///     Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public override void Delete(TEntity entity)
        {
            EntityChangeEventHelper.TriggerEntityDeletingEvent(entity);
            Session.Delete(entity);
            EntityChangeEventHelper.TriggerEntityDeletedEventOnUowCompleted(entity);
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
