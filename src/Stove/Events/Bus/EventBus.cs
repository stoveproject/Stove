using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Castle.DynamicProxy.Generators.Emitters.SimpleAST;

using Stove.Events.Bus.Factories;
using Stove.Events.Bus.Factories.Internals;
using Stove.Events.Bus.Handlers;
using Stove.Events.Bus.Handlers.Internals;
using Stove.Extensions;
using Stove.Log;
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

        /// <summary>
        ///     Creates a new <see cref="EventBus" /> instance.
        ///     Instead of creating a new instace, you can use <see cref="Default" /> to use Global <see cref="EventBus" />.
        /// </summary>
        public EventBus()
        {
            _handlerFactories = new ConcurrentDictionary<Type, List<IEventHandlerFactory>>();

            Logger = NullLogger.Instance;
        }

        /// <summary>
        ///     Reference to the Logger.
        /// </summary>
        public ILogger Logger { get; set; }

        /// <inheritdoc />
        public IDisposable Register<TEvent>(Action<TEvent> action) where TEvent : IEvent
        {
            return Register(typeof(TEvent), new ActionEventHandler<TEvent>(action));
        }

        /// <inheritdoc />
        public IDisposable Register<TEvent>(IEventHandler<TEvent> handler) where TEvent : IEvent
        {
            return Register(typeof(TEvent), handler);
        }

        /// <inheritdoc />
        public IDisposable Register<TEvent, THandler>()
            where TEvent : IEvent
            where THandler : IEventHandler<TEvent>, new()
        {
            return Register(typeof(TEvent), new TransientEventHandlerFactory<THandler>());
        }

        /// <inheritdoc />
        public IDisposable Register(Type eventType, IEventHandler handler)
        {
            return Register(eventType, new SingleInstanceHandlerFactory(handler));
        }

        /// <inheritdoc />
        public IDisposable Register<TEvent>(IEventHandlerFactory handlerFactory) where TEvent : IEvent
        {
            return Register(typeof(TEvent), handlerFactory);
        }

        /// <inheritdoc />
        public IDisposable Register(Type eventType, IEventHandlerFactory handlerFactory)
        {
            GetOrCreateHandlerFactories(eventType)
                .Locking(factories => factories.Add(handlerFactory));

            return new FactoryUnregistrar(this, eventType, handlerFactory);
        }

        /// <inheritdoc />
        public void Unregister<TEvent>(Action<TEvent> action) where TEvent : IEvent
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

        /// <inheritdoc />
        public void Unregister<TEvent>(IEventHandler<TEvent> handler) where TEvent : IEvent
        {
            Unregister(typeof(TEvent), handler);
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public void Unregister<TEvent>(IEventHandlerFactory factory) where TEvent : IEvent
        {
            Unregister(typeof(TEvent), factory);
        }

        /// <inheritdoc />
        public void Unregister(Type eventType, IEventHandlerFactory factory)
        {
            GetOrCreateHandlerFactories(eventType).Locking(factories => factories.Remove(factory));
        }

        /// <inheritdoc />
        public void UnregisterAll<TEvent>() where TEvent : IEvent
        {
            UnregisterAll(typeof(TEvent));
        }

        /// <inheritdoc />
        public void UnregisterAll(Type @event)
        {
            GetOrCreateHandlerFactories(@event).Locking(factories => factories.Clear());
        }

        /// <inheritdoc />
        public void Publish<TEvent>(TEvent @event) where TEvent : IEvent
        {
            Publish(typeof(TEvent), @event);
        }

        /// <inheritdoc />
        public void Publish(Type eventType, IEvent @event)
        {
            var exceptions = new List<Exception>();

            PublishHandlingException(eventType, @event, exceptions);

            if (exceptions.Any())
            {
                if (exceptions.Count == 1)
                {
                    exceptions[0].ReThrow();
                }

                throw new AggregateException("More than one error has occurred while publishing the event: " + eventType, exceptions);
            }
        }

        public Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent
        {
            ExecutionContext.SuppressFlow();

            Task task = Task.Run(
                () =>
                {
                    try
                    {
                        Publish(@event);
                    }
                    catch (Exception ex)
                    {
                        Logger.Warn(ex.ToString(), ex);
                    }
                });

            ExecutionContext.RestoreFlow();

            return task;
        }

        /// <inheritdoc />
        public Task PublishAsync(Type eventType, IEvent @event)
        {
            ExecutionContext.SuppressFlow();

            Task task = Task.Run(
                () =>
                {
                    try
                    {
                        Publish(eventType, @event);
                    }
                    catch (Exception ex)
                    {
                        Logger.Warn(ex.ToString(), ex);
                    }
                });

            ExecutionContext.RestoreFlow();

            return task;
        }

        private void PublishHandlingException(Type eventType, IEvent @event, List<Exception> exceptions)
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
                                                      new[] { eventType });

                                                  method.Invoke(handler, new object[] { @event });
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
