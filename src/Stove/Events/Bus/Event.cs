using System;

using Stove.Timing;

namespace Stove.Events.Bus
{
    [Serializable]
    public abstract class Event : IEvent
    {
        protected Event()
        {
            EventTime = Clock.Now;
        }

        public DateTime EventTime { get; set; }
    }
}
