namespace Stove.Events.Bus.Entities
{
    public class Envelope
    {
        public Envelope(IEvent @event, EventHeaders headers)
        {
            Event = @event;
            Headers = headers;
        }

        public EventHeaders Headers { get; }

        public IEvent Event { get; }
    }
}
