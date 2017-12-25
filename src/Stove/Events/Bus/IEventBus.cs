using System;
using System.Threading.Tasks;

using Stove.Events.Bus.Factories;
using Stove.Events.Bus.Handlers;

namespace Stove.Events.Bus
{
    /// <summary>
    ///     Defines interface of the event bus.
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        ///     Registers to an event.
        ///     Given action is called for all event occurrences.
        /// </summary>
        /// <param name="action">Action to handle events</param>
        /// <typeparam name="TEvent">Event type</typeparam>
        IDisposable Register<TEvent>(Action<TEvent> action) where TEvent : IEvent;

        /// <summary>
        ///     Registers to an event.
        ///     Same (given) instance of the handler is used for all event occurrences.
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        /// <param name="handler">Object to handle the event</param>
        IDisposable Register<TEvent>(IEventHandler<TEvent> handler) where TEvent : IEvent;

        /// <summary>
        ///     Registers to an event.
        ///     A new instance of <see cref="THandler" /> object is created for every event occurrence.
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        /// <typeparam name="THandler">Type of the event handler</typeparam>
        IDisposable Register<TEvent, THandler>() where TEvent : IEvent where THandler : IEventHandler<TEvent>, new();

        /// <summary>
        ///     Registers to an event.
        ///     Same (given) instance of the handler is used for all event occurrences.
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="handler">Object to handle the event</param>
        IDisposable Register(Type eventType, IEventHandler handler);

        /// <summary>
        ///     Registers to an event.
        ///     Given factory is used to create/release handlers
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        /// <param name="handlerFactory">A factory to create/release handlers</param>
        IDisposable Register<TEvent>(IEventHandlerFactory handlerFactory) where TEvent : IEvent;

        /// <summary>
        ///     Registers to an event.
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="handlerFactory">A factory to create/release handlers</param>
        IDisposable Register(Type eventType, IEventHandlerFactory handlerFactory);

        /// <summary>
        ///     Unregisters from an event.
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        /// <param name="action"></param>
        void Unregister<TEvent>(Action<TEvent> action) where TEvent : IEvent;

        /// <summary>
        ///     Unregisters from an event.
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        /// <param name="handler">Handler object that is registered before</param>
        void Unregister<TEvent>(IEventHandler<TEvent> handler) where TEvent : IEvent;

        /// <summary>
        ///     Unregisters from an event.
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="handler">Handler object that is registered before</param>
        void Unregister(Type eventType, IEventHandler handler);

        /// <summary>
        ///     Unregisters from an event.
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        /// <param name="factory">Factory object that is registered before</param>
        void Unregister<TEvent>(IEventHandlerFactory factory) where TEvent : IEvent;

        /// <summary>
        ///     Unregisters from an event.
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="factory">Factory object that is registered before</param>
        void Unregister(Type eventType, IEventHandlerFactory factory);

        /// <summary>
        ///     Unregisters all event handlers of given event type.
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        void UnregisterAll<TEvent>() where TEvent : IEvent;

        /// <summary>
        ///     Unregisters all event handlers of given event type.
        /// </summary>
        /// <param name="event">Event type</param>
        void UnregisterAll(Type @event);

        /// <summary>
        ///     Publishes an event.
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        /// <param name="event">Related data for the event</param>
        void Publish<TEvent>(TEvent @event) where TEvent : IEvent;

        /// <summary>
        ///     Publishes an event.
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="event">Related data for the event</param>
        void Publish(Type eventType, IEvent @event);

        /// <summary>
        ///     Publishes an event asynchronously.
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        /// <param name="event">Related data for the event</param>
        /// <returns>The task to handle async operation</returns>
        Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent;

        /// <summary>
        ///     Publishes an event asynchronously.
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="event">Related data for the event</param>
        /// <returns>The task to handle async operation</returns>
        Task PublishAsync(Type eventType, IEvent @event);
    }
}
