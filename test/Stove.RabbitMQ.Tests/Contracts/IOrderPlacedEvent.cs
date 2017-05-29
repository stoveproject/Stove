namespace Stove.RabbitMQ.Tests.Contracts
{
    public interface IOrderPlacedEvent
    {
        int OrderId { get; set; }
    }
}
