using System;

namespace Stove.Events.Bus.Entities
{
    /// <summary>
    /// This type of event is used to notify just before deletion of an Entity.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    [Serializable]
    public class EntityDeletingEvent<TEntity> : EntityChangingEvent<TEntity>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="entity">The entity which is being deleted</param>
        public EntityDeletingEvent(TEntity entity)
            : base(entity)
        {

        }
    }
}