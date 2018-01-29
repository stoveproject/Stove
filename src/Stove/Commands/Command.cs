namespace Stove.Commands
{
    public abstract class Command : IMessage
    {
        public string CorrelationId { get; set; }
    }
}
