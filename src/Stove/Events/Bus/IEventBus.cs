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
        /// <typeparam name="TEventData">Event type</typeparam>
        IDisposable Register<TEventData>(Action<TEventData> action) where TEventData : IEventData;

        /// <summary>
        ///     Registers to an event.
        ///     Same (given) instance of the handler is used for all event occurrences.
        /// </summary>
        /// <typeparam name="TEventData">Event type</typeparam>
        /// <param name="handler">Object to handle the event</param>
        IDisposable Register<TEventData>(IEventHandler<TEventData> handler) where TEventData : IEventData;

        /// <summary>
        ///     Registers to an event.
        ///     A new instance of <see cref="THandler" /> object is created for every event occurrence.
        /// </summary>
        /// <typeparam name="TEventData">Event type</typeparam>
        /// <typeparam name="THandler">Type of the event handler</typeparam>
        IDisposable Register<TEventData, THandler>() where TEventData : IEventData where THandler : IEventHandler<TEventData>, new();

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
        /// <typeparam name="TEventData">Event type</typeparam>
        /// <param name="handlerFactory">A factory to create/release handlers</param>
        IDisposable Register<TEventData>(IEventHandlerFactory handlerFactory) where TEventData : IEventData;

        /// <summary>
        ///     Registers to an event.
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="handlerFactory">A factory to create/release handlers</param>
        IDisposable Register(Type eventType, IEventHandlerFactory handlerFactory);

        /// <summary>
        ///     Unregisters from an event.
        /// </summary>
        /// <typeparam name="TEventData">Event type</typeparam>
        /// <param name="action"></param>
        void Unregister<TEventData>(Action<TEventData> action) where TEventData : IEventData;

        /// <summary>
        ///     Unregisters from an event.
        /// </summary>
        /// <typeparam name="TEventData">Event type</typeparam>
        /// <param name="handler">Handler object that is registered before</param>
        void Unregister<TEventData>(IEventHandler<TEventData> handler) where TEventData : IEventData;

        /// <summary>
        ///     Unregisters from an event.
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="handler">Handler object that is registered before</param>
        void Unregister(Type eventType, IEventHandler handler);

        /// <summary>
        ///     Unregisters from an event.
        /// </summary>
        /// <typeparam name="TEventData">Event type</typeparam>
        /// <param name="factory">Factory object that is registered before</param>
        void Unregister<TEventData>(IEventHandlerFactory factory) where TEventData : IEventData;

        /// <summary>
        ///     Unregisters from an event.
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="factory">Factory object that is registered before</param>
        void Unregister(Type eventType, IEventHandlerFactory factory);

        /// <summary>
        ///     Unregisters all event handlers of given event type.
        /// </summary>
        /// <typeparam name="TEventData">Event type</typeparam>
        void UnregisterAll<TEventData>() where TEventData : IEventData;

        /// <summary>
        ///     Unregisters all event handlers of given event type.
        /// </summary>
        /// <param name="event">Event type</param>
        void UnregisterAll(Type @event);

        /// <summary>
        ///     Publishes an event.
        /// </summary>
        /// <typeparam name="TEventData">Event type</typeparam>
        /// <param name="event">Related data for the event</param>
        void Publish<TEventData>(TEventData @event) where TEventData : IEventData;

        /// <summary>
        ///     Publishes an event.
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="event">Related data for the event</param>
        void Publish(Type eventType, IEventData @event);

        /// <summary>
        ///     Publishes an event asynchronously.
        /// </summary>
        /// <typeparam name="TEventData">Event type</typeparam>
        /// <param name="event">Related data for the event</param>
        /// <returns>The task to handle async operation</returns>
        Task PublishAsync<TEventData>(TEventData @event) where TEventData : IEventData;

        /// <summary>
        ///     Publishes an event asynchronously.
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="event">Related data for the event</param>
        /// <returns>The task to handle async operation</returns>
        Task PublishAsync(Type eventType, IEventData @event);
    }
}
