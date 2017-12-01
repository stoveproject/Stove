using System.Collections.Generic;
using System.Linq;

namespace Stove.Domain.Entities
{
    public class AggregateRoot : AggregateRoot<int>, IAggregateRoot
    {
    }

    public class AggregateRoot<TPrimaryKey> : Entity<TPrimaryKey>, IAggregateRoot<TPrimaryKey>
    {
        private readonly EventRecorder _recorder;

        public AggregateRoot()
        {
            _recorder = new EventRecorder();
        }

        /// <summary>
        ///     Determines whether this instance has state changes.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if this instance has state changes; otherwise, <c>false</c>.
        /// </returns>
        public bool HasChanges()
        {
            return _recorder.Any();
        }

        /// <summary>
        ///     Gets the state changes applied to this instance.
        /// </summary>
        /// <returns>A list of recorded state changes.</returns>
        public IEnumerable<object> GetChanges()
        {
            return _recorder.ToArray();
        }

        /// <summary>
        ///     Clears the state changes.
        /// </summary>
        public void ClearChanges()
        {
            _recorder.Reset();
        }

        protected override void AfterApplyChange(object @event)
        {
            _recorder.Record(@event);
        }
    }
}
