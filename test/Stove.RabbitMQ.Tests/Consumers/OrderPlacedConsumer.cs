using System.Threading.Tasks;

using Autofac.Extras.IocManager;

using MassTransit;

using Stove.RabbitMQ.Tests.Contracts;

namespace Stove.RabbitMQ.Tests.Consumers
{
    public class OrderPlacedConsumer : IConsumer<OrderPlacedEvent>, ITransientDependency
    {
        public Task Consume(ConsumeContext<OrderPlacedEvent> context)
        {
            int orderId = context.Message.OrderId;

            return Task.FromResult(0);
        }
    }
}
