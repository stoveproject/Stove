using System;

using Stove.Domain.Entities;

namespace Stove.Events.Bus.Entities
{
    /// <summary>
    /// Used to pass data for an event when an entity (<see cref="IEntity"/>) is being changed (creating, updating or deleting).
    /// See <see cref="EntityCreatingEvent{TEntity}"/>, <see cref="EntityDeletingEvent{TEntity}"/> and <see cref="EntityUpdatingEvent{TEntity}"/> classes.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    [Serializable]
    public class EntityChangingEvent<TEntity> : EntityEvent<TEntity>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="entity">Changing entity in this event</param>
        public EntityChangingEvent(TEntity entity)
            : base(entity)
        {

        }
    }
}