using System;
using System.Threading.Tasks;

namespace Stove.MQ
{
    public interface IMessageBus
    {
        Task Publish<TMessage>(TMessage message) where TMessage : class;

        Task Publish<TMessage>(object message) where TMessage : class;

        Task Publish(object message);

        Task Publish(object message, Type messageType);

        Task<TResponse> CallRequest<TRequest, TResponse>(TRequest request, TimeSpan timeout, string queueName) where TRequest : class where TResponse : class;

        Task<TResponse> CallRequest<TRequest, TResponse>(TRequest request, TimeSpan timeout, Uri queueUri) where TRequest : class where TResponse : class;
    }
}
