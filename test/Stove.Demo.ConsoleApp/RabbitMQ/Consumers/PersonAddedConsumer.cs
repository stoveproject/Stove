using System.Threading.Tasks;

using Autofac.Extras.IocManager;

using MassTransit;

using Stove.Demo.ConsoleApp.RabbitMQ.Messages;

namespace Stove.Demo.ConsoleApp.RabbitMQ.Consumers
{
    public class PersonAddedConsumer : IConsumer<PersonMessage>, ITransientDependency
    {
        public Task Consume(ConsumeContext<PersonMessage> context)
        {
            string name = context.Message.Name;

            return Task.FromResult(0);
        }
    }
}
