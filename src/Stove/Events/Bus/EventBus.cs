using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Stove.Events.Bus.Factories;
using Stove.Events.Bus.Factories.Internals;
using Stove.Events.Bus.Handlers;
using Stove.Events.Bus.Handlers.Internals;
using Stove.Extensions;
using Stove.Threading.Extensions;

namespace Stove.Events.Bus
{
    /// <summary>
    ///     Implements EventBus as Singleton pattern.
    /// </summary>
    public class EventBus : IEventBus
    {
        /// <summary>
        ///     All registered handler factories.
        ///     Key: Type of the event
        ///     Value: List of handler factories
        /// </summary>
        private readonly ConcurrentDictionary<Type, List<IEventHandlerFactory>> _handlerFactories;
        private EventPublishingBehaviour _behaviour;

        /// <summary>
        ///     Creates a new <see cref="EventBus" /> instance.
        /// </summary>
        public EventBus()
        {
            _handlerFactories = new ConcurrentDictionary<Type, List<IEventHandlerFactory>>();
        }

        public IDisposable Register<TEvent>(Action<TEvent, EventHeaders> action) where TEvent : IEvent
        {
            return Register(typeof(TEvent), new ActionEventHandler<TEvent>(action));
        }

        public void RegisterPublishingBehaviour(EventPublishingBehaviour behaviour)
        {
            _behaviour += behaviour;
        }

        public IDisposable Register<TEvent>(IEventHandler<TEvent> handler) where TEvent : IEvent
        {
            return Register(typeof(TEvent), handler);
        }

        public IDisposable Register<TEvent, THandler>()
            where TEvent : IEvent
            where THandler : IEventHandler<TEvent>, new()
        {
            return Register(typeof(TEvent), new TransientEventHandlerFactory<THandler>());
        }

        public IDisposable Register(Type eventType, IEventHandler handler)
        {
            return Register(eventType, new SingleInstanceHandlerFactory(handler));
        }

        public IDisposable Register<TEvent>(IEventHandlerFactory handlerFactory) where TEvent : IEvent
        {
            return Register(typeof(TEvent), handlerFactory);
        }

        public IDisposable Register(Type eventType, IEventHandlerFactory handlerFactory)
        {
            GetOrCreateHandlerFactories(eventType)
                .Locking(factories => factories.Add(handlerFactory));

            return new FactoryUnregistrar(this, eventType, handlerFactory);
        }

        public void Unregister<TEvent>(Action<TEvent, EventHeaders> action) where TEvent : IEvent
        {
            Check.NotNull(action, nameof(action));

            GetOrCreateHandlerFactories(typeof(TEvent))
                .Locking(factories =>
                {
                    factories.RemoveAll(
                        factory =>
                        {
                            if (!(factory is SingleInstanceHandlerFactory singleInstanceFactory))
                            {
                                return false;
                            }

                            if (!(singleInstanceFactory.HandlerInstance is ActionEventHandler<TEvent> actionHandler))
                            {
                                return false;
                            }

                            return actionHandler.Action == action;
                        });
                });
        }

        public void Unregister<TEvent>(IEventHandler<TEvent> handler) where TEvent : IEvent
        {
            Unregister(typeof(TEvent), handler);
        }

        public void Unregister(Type eventType, IEventHandler handler)
        {
            GetOrCreateHandlerFactories(eventType)
                .Locking(factories =>
                {
                    factories.RemoveAll(
                        factory =>
                            factory is SingleInstanceHandlerFactory &&
                            (factory as SingleInstanceHandlerFactory).HandlerInstance == handler
                        );
                });
        }

        public void Unregister<TEvent>(IEventHandlerFactory factory) where TEvent : IEvent
        {
            Unregister(typeof(TEvent), factory);
        }

        public void Unregister(Type eventType, IEventHandlerFactory factory)
        {
            GetOrCreateHandlerFactories(eventType).Locking(factories => factories.Remove(factory));
        }

        public void UnregisterAll<TEvent>() where TEvent : IEvent
        {
            UnregisterAll(typeof(TEvent));
        }

        public void UnregisterAll(Type @event)
        {
            GetOrCreateHandlerFactories(@event).Locking(factories => factories.Clear());
        }

        public void Publish<TEvent>(TEvent @event, EventHeaders headers) where TEvent : IEvent
        {
            Publish(typeof(TEvent), @event, headers);
        }

        public void Publish(Type eventType, IEvent @event, EventHeaders headers)
        {
            var exceptions = new List<Exception>();

            _behaviour?.Invoke(@event, headers);

            PublishHandlingException(eventType, @event, headers, exceptions);

            if (exceptions.Any())
            {
                if (exceptions.Count == 1)
                {
                    exceptions[0].ReThrow();
                }

                throw new AggregateException("More than one error has occurred while publishing the event: " + eventType, exceptions);
            }
        }

        public Task PublishAsync<TEvent>(TEvent @event, EventHeaders headers, CancellationToken cancellationToken = default) where TEvent : IEvent
        {
            return Task.Run(() => { Publish(@event, headers); }, cancellationToken);
        }

        public Task PublishAsync(Type eventType, IEvent @event, EventHeaders headers, CancellationToken cancellationToken = default)
        {
            return Task.Run(() => { Publish(eventType, @event, headers); }, cancellationToken);
        }

        private void PublishHandlingException(Type eventType, IEvent @event, EventHeaders headers, List<Exception> exceptions)
        {
            GetHandlerFactories(eventType).SelectMany(x => x.EventHandlerFactories)
                                          .ForEach(f =>
                                          {
                                              IEventHandler handler = f.GetHandler();
                                              try
                                              {
                                                  Type handlerType = typeof(IEventHandler<>).MakeGenericType(eventType);
                                                  MethodInfo method = handlerType.GetMethod(
                                                      "Handle",
                                                      new[] { eventType, typeof(EventHeaders) });

                                                  method.Invoke(handler, new object[] { @event, headers });
                                              }
                                              catch (TargetInvocationException ex)
                                              {
                                                  exceptions.Add(ex.InnerException);
                                              }
                                              catch (Exception ex)
                                              {
                                                  exceptions.Add(ex);
                                              }
                                              finally
                                              {
                                                  f.ReleaseHandler(handler);
                                              }
                                          });
        }

        private IEnumerable<EventTypeWithEventHandlerFactories> GetHandlerFactories(Type eventType)
        {
            var handlerFactoryList = new List<EventTypeWithEventHandlerFactories>();

            foreach (KeyValuePair<Type, List<IEventHandlerFactory>> handlerFactory in _handlerFactories.Where(hf => ShouldPublishEventForHandler(eventType, hf.Key)))
            {
                handlerFactoryList.Add(new EventTypeWithEventHandlerFactories(handlerFactory.Key, handlerFactory.Value));
            }

            return handlerFactoryList.ToArray();
        }

        private static bool ShouldPublishEventForHandler(Type eventType, Type handlerType)
        {
            //Should publish same type
            if (handlerType == eventType)
            {
                return true;
            }

            //Should publish for inherited types
            if (handlerType.IsAssignableFrom(eventType))
            {
                return true;
            }

            return false;
        }

        private List<IEventHandlerFactory> GetOrCreateHandlerFactories(Type eventType)
        {
            if (!_handlerFactories.TryGetValue(eventType, out List<IEventHandlerFactory> factories))
            {
                factories = new List<IEventHandlerFactory>();
            }

            return _handlerFactories.GetOrAdd(eventType, factories);
        }

        private class EventTypeWithEventHandlerFactories
        {
            public EventTypeWithEventHandlerFactories(Type eventType, List<IEventHandlerFactory> eventHandlerFactories)
            {
                EventType = eventType;
                EventHandlerFactories = eventHandlerFactories;
            }

            public Type EventType { get; }

            public List<IEventHandlerFactory> EventHandlerFactories { get; }
        }
    }
}
