using System;

namespace Stove.MQ
{
    public interface IMessageBus
    {
        void Publish<TMessage>(TMessage message) where TMessage : class;

        void Publish<TMessage>(object message) where TMessage : class;

        void Publish(object message);

        void Publish(object message, Type messageType);
    }
}
