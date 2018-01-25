using System;
using System.Threading.Tasks;

using Stove.Events.Bus.Factories;
using Stove.Events.Bus.Handlers;
using Stove.Utils.Etc;

namespace Stove.Events.Bus
{
    /// <summary>
    ///     An event bus that implements Null object pattern.
    /// </summary>
    public sealed class NullEventBus : IEventBus
    {
        private NullEventBus()
        {
        }

        /// <summary>
        ///     Gets single instance of <see cref="NullEventBus" /> class.
        /// </summary>
        public static NullEventBus Instance { get; } = new NullEventBus();

        /// <inheritdoc />
        public IDisposable Register<TEvent>(Action<TEvent> action) where TEvent : IEvent
        {
            return NullDisposable.Instance;
        }

        /// <inheritdoc />
        public IDisposable Register<TEvent>(IEventHandler<TEvent> handler) where TEvent : IEvent
        {
            return NullDisposable.Instance;
        }

        /// <inheritdoc />
        public IDisposable Register<TEvent, THandler>()
            where TEvent : IEvent
            where THandler : IEventHandler<TEvent>, new()
        {
            return NullDisposable.Instance;
        }

        /// <inheritdoc />
        public IDisposable Register(Type eventType, IEventHandler handler)
        {
            return NullDisposable.Instance;
        }

        /// <inheritdoc />
        public IDisposable Register<TEvent>(IEventHandlerFactory handlerFactory) where TEvent : IEvent
        {
            return NullDisposable.Instance;
        }

        /// <inheritdoc />
        public IDisposable Register(Type eventType, IEventHandlerFactory handlerFactory)
        {
            return NullDisposable.Instance;
        }

        /// <inheritdoc />
        public void Unregister<TEvent>(Action<TEvent> action) where TEvent : IEvent
        {
        }

        /// <inheritdoc />
        public void Unregister<TEvent>(IEventHandler<TEvent> handler) where TEvent : IEvent
        {
        }

        /// <inheritdoc />
        public void Unregister(Type eventType, IEventHandler handler)
        {
        }

        /// <inheritdoc />
        public void Unregister<TEvent>(IEventHandlerFactory factory) where TEvent : IEvent
        {
        }

        /// <inheritdoc />
        public void Unregister(Type eventType, IEventHandlerFactory factory)
        {
        }

        /// <inheritdoc />
        public void UnregisterAll<TEvent>() where TEvent : IEvent
        {
        }

        /// <inheritdoc />
        public void UnregisterAll(Type @event)
        {
        }

        /// <inheritdoc />
        public void Publish<TEvent>(TEvent @event) where TEvent : IEvent
        {
        }

        /// <inheritdoc />
        public void Publish(Type eventType, IEvent @event)
        {
        }

        /// <inheritdoc />
        public Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent
        {
            return new Task(() => { });
        }

        /// <inheritdoc />
        public Task PublishAsync(Type eventType, IEvent @event)
        {
            return new Task(() => { });
        }
    }
}
