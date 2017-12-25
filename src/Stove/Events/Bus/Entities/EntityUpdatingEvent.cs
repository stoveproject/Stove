using System;

namespace Stove.Events.Bus.Entities
{
    /// <summary>
    /// This type of event is used to notify just before update of an Entity.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    [Serializable]
    public class EntityUpdatingEvent<TEntity> : EntityChangingEvent<TEntity>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="entity">The entity which is being updated</param>
        public EntityUpdatingEvent(TEntity entity)
            : base(entity)
        {

        }
    }
}