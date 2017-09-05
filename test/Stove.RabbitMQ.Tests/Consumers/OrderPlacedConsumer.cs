using System.Threading.Tasks;

using MassTransit;

using Stove.MQ;
using Stove.RabbitMQ.Tests.Contracts;

namespace Stove.RabbitMQ.Tests.Consumers
{
    public class OrderPlacedConsumer : ConsumerBase, IConsumer<OrderPlacedEvent>
    {
        public Task Consume(ConsumeContext<OrderPlacedEvent> context)
        {
            int orderId = context.Message.OrderId;

            return Task.FromResult(0);
        }
    }
}
