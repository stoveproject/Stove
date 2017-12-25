using System;

using Stove.Domain.Entities;

namespace Stove.Events.Bus.Entities
{
    /// <summary>
    /// Used to pass data for an event when an entity (<see cref="IEntity"/>) is changed (created, updated or deleted).
    /// See <see cref="EntityCreatedEvent{TEntity}"/>, <see cref="EntityDeletedEvent{TEntity}"/> and <see cref="EntityUpdatedEvent{TEntity}"/> classes.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    [Serializable]
    public class EntityChangedEvent<TEntity> : EntityEvent<TEntity>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="entity">Changed entity in this event</param>
        public EntityChangedEvent(TEntity entity)
            : base(entity)
        {

        }
    }
}