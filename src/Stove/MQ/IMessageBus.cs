using System;
using System.Threading.Tasks;

namespace Stove.MQ
{
    public interface IMessageBus
    {
        void Publish<TMessage>(TMessage message) where TMessage : class;

        void Publish<TMessage>(object message) where TMessage : class;

        void Publish(object message);

        void Publish(object message, Type messageType);

        Task<TResponse> CallRequest<TRequest, TResponse>(TRequest request, TimeSpan timeout, string queueName) where TRequest : class where TResponse : class;

        Task<TResponse> CallRequest<TRequest, TResponse>(TRequest request, TimeSpan timeout, Uri queueUri) where TRequest : class where TResponse : class;
    }
}
