using System;

using Stove.Timing;

namespace Stove.Events.Bus
{
    /// <summary>
    ///     Implements <see cref="IEvent" /> and provides a base for event data classes.
    /// </summary>
    [Serializable]
    public abstract class Event : IEvent
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        protected Event()
        {
            EventTime = Clock.Now;
        }

        /// <summary>
        ///     The time when the event occurred.
        /// </summary>
        public DateTime EventTime { get; set; }
    }
}
