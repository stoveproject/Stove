namespace Stove
{
    public abstract class Command : IMessage
    {
        public string CorrelationId { get; set; }
    }
}
