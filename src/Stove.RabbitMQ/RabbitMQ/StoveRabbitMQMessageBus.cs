using System;
using System.Threading.Tasks;

using MassTransit;

using Stove.MQ;

namespace Stove.RabbitMQ
{
    public class StoveRabbitMQMessageBus : IMessageBus
    {
        private readonly IBus _bus;
        private readonly IStoveRabbitMQConfiguration _stoveRabbitMqConfiguration;

        public StoveRabbitMQMessageBus(IBus bus,
            IStoveRabbitMQConfiguration stoveRabbitMqConfiguration)
        {
            _bus = bus;
            _stoveRabbitMqConfiguration = stoveRabbitMqConfiguration;
        }

        public Task Publish<TMessage>(TMessage message) where TMessage : class
        {
            return _bus.Publish(message);
        }

        public Task Publish<TMessage>(object message) where TMessage : class
        {
            return _bus.Publish<TMessage>(message);
        }

        public Task Publish(object message)
        {
            return _bus.Publish(message);
        }

        public Task Publish(object message, Type messageType)
        {
            return _bus.Publish(message, messageType);
        }

        public Task<TResponse> CallRequest<TRequest, TResponse>(TRequest request, TimeSpan timeout, string queueName)
            where TRequest : class
            where TResponse : class
        {
            return _bus.CreateRequestClient<TRequest, TResponse>(new Uri(new Uri(_stoveRabbitMqConfiguration.HostAddress), queueName), timeout).Request(request);
        }

        public Task<TResponse> CallRequest<TRequest, TResponse>(TRequest request, TimeSpan timeout, Uri queueUri)
            where TRequest : class
            where TResponse : class
        {
            return _bus.CreateRequestClient<TRequest, TResponse>(queueUri, timeout).Request(request);
        }
    }
}
