using System;

namespace Stove.Events.Bus
{
    /// <summary>
    ///     Defines interface for all Event data classes.
    /// </summary>
    public interface IEvent
    {
        /// <summary>
        ///     The time when the event occured.
        /// </summary>
        DateTime EventTime { get; set; }
    }
}
