using System.Linq;

using Raven.Client;

using Stove.Domain.Entities;
using Stove.Domain.Repositories;

namespace Stove.RavenDB.Repositories
{
    public class RavenDBRepositoryBase<TEntity, TPrimaryKey> : StoveRepositoryBase<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
    {
        public IDocumentSession Session { get { return _sessionProvider.Session; } }
        private readonly ISessionProvider _sessionProvider;

        public RavenDBRepositoryBase(ISessionProvider sessionProvider)
        {
            _sessionProvider = sessionProvider;
        }

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
            Session.Store(entity);
            return entity;
        }

        /// <summary>
        ///     Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public override void Delete(TEntity entity)
        {
            Session.Delete(entity);
        }

        /// <summary>
        ///     Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public override void Delete(TPrimaryKey id)
        {
            Session.Delete(id);
        }
    }
}
