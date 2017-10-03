using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

using MassTransit;

using Stove.MQ;

namespace Stove.RabbitMQ
{
    public class StoveRabbitMQMessageBus : IMessageBus
    {
        [NotNull]
        private readonly IBus _bus;

        private readonly IStoveRabbitMQConfiguration _stoveRabbitMqConfiguration;

        public StoveRabbitMQMessageBus([NotNull] IBus bus,
            IStoveRabbitMQConfiguration stoveRabbitMqConfiguration)
        {
            _bus = bus;
            _stoveRabbitMqConfiguration = stoveRabbitMqConfiguration;
        }

        public void Publish<TMessage>(TMessage message) where TMessage : class
        {
            _bus.Publish(message);
        }

        public void Publish<TMessage>(object message) where TMessage : class
        {
            _bus.Publish<TMessage>(message);
        }

        public void Publish(object message)
        {
            _bus.Publish(message);
        }

        public void Publish(object message, Type messageType)
        {
            _bus.Publish(message, messageType);
        }
        public Task<TResponse> CallRequest<TRequest, TResponse>(TRequest request, TimeSpan timeOut,string queueName)
            where TRequest : class
            where TResponse : class
        {
            return _bus.CreateRequestClient<TRequest, TResponse>(new Uri($"{_stoveRabbitMqConfiguration.HostAddress}{queueName}"), timeOut).Request(request);
        }
    }
}
