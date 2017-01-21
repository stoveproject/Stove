using System;

using MassTransit;

namespace Stove.Demo.ConsoleApp.RabbitMQ.Messages
{
    public interface IPersonAddedMessage : CorrelatedBy<Guid>
    {
        string Name { get; set; }
    }

    public class PersonAddedMessage : IPersonAddedMessage
    {
        public string Name { get; set; }

        public Guid CorrelationId { get; set; }
    }
}
