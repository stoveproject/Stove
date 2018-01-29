namespace Stove.Events.Bus.Entities
{
    public class Envelope
    {
        public Envelope(IMessage message, Headers headers)
        {
            Message = message;
            Headers = headers;
        }

        public Headers Headers { get; }

        public IMessage Message { get; }
    }
}
