using System;

namespace Stove.Events.Bus
{
    /// <summary>
    /// Defines interface for all Event data classes.
    /// </summary>
    public interface IEventData
    {
        /// <summary>
        /// The time when the event occured.
        /// </summary>
        DateTime EventTime { get; set; }

        /// <summary>
        /// The object which publishes the event (optional).
        /// </summary>
        object EventSource { get; set; }
    }
}
