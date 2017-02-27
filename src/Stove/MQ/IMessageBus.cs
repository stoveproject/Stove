using System;

using JetBrains.Annotations;

namespace Stove.MQ
{
    public interface IMessageBus
    {
        void Publish<TMessage>([NotNull] TMessage message) where TMessage : class;

        void Publish<TMessage>([NotNull] object message) where TMessage : class;

        void Publish([NotNull] object message);

        void Publish([NotNull] object message, [NotNull] Type messageType);
    }
}
