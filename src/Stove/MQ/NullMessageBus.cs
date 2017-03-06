using System;
using System.Diagnostics.CodeAnalysis;

namespace Stove.MQ
{
    [ExcludeFromCodeCoverage]
    public class NullMessageBus : IMessageBus
    {
        public static readonly NullMessageBus Instance = new NullMessageBus();

        public void Publish<TMessage>(TMessage message) where TMessage : class
        {
        }

        public void Publish<TMessage>(object message) where TMessage : class
        {
        }

        public void Publish(object message)
        {
        }

        public void Publish(object message, Type messageType)
        {
        }
    }
}
