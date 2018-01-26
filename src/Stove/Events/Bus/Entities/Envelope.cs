using System.Collections.Generic;

namespace Stove.Events.Bus.Entities
{
    public class Envelope
    {
        public Envelope(IEvent @event, Dictionary<string, object> headers)
        {
            Event = @event;
            Headers = headers;
        }

        public Dictionary<string, object> Headers { get; }

        public IEvent Event { get; }
    }
}
