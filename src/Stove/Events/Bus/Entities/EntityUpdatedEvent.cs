using System;

namespace Stove.Events.Bus.Entities
{
    /// <summary>
    /// This type of event can be used to notify just after update of an Entity.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    [Serializable]
    public class EntityUpdatedEvent<TEntity> : EntityChangedEvent<TEntity>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="entity">The entity which is updated</param>
        public EntityUpdatedEvent(TEntity entity)
            : base(entity)
        {

        }
    }
}