namespace Stove.RabbitMQ.Tests.Contracts
{
    public class OrderPlacedEvent : IOrderPlacedEvent
    {
        public int OrderId { get; set; }
    }
}
