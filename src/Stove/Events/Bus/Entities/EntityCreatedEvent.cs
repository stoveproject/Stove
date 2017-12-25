using System;

namespace Stove.Events.Bus.Entities
{
    /// <summary>
    /// This type of event can be used to notify just after creation of an Entity.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    [Serializable]
    public class EntityCreatedEvent<TEntity> : EntityChangedEvent<TEntity>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="entity">The entity which is created</param>
        public EntityCreatedEvent(TEntity entity)
            : base(entity)
        {

        }
    }
}