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
		public IDisposable Register<TEventData>(Action<TEventData> action) where TEventData : IEventData
		{
			return NullDisposable.Instance;
		}

		/// <inheritdoc />
		public IDisposable Register<TEventData>(IEventHandler<TEventData> handler) where TEventData : IEventData
		{
			return NullDisposable.Instance;
		}

		/// <inheritdoc />
		public IDisposable Register<TEventData, THandler>()
			where TEventData : IEventData
			where THandler : IEventHandler<TEventData>, new()
		{
			return NullDisposable.Instance;
		}

		/// <inheritdoc />
		public IDisposable Register(Type eventType, IEventHandler handler)
		{
			return NullDisposable.Instance;
		}

		/// <inheritdoc />
		public IDisposable Register<TEventData>(IEventHandlerFactory handlerFactory) where TEventData : IEventData
		{
			return NullDisposable.Instance;
		}

		/// <inheritdoc />
		public IDisposable Register(Type eventType, IEventHandlerFactory handlerFactory)
		{
			return NullDisposable.Instance;
		}

		/// <inheritdoc />
		public void Unregister<TEventData>(Action<TEventData> action) where TEventData : IEventData
		{
		}

		/// <inheritdoc />
		public void Unregister<TEventData>(IEventHandler<TEventData> handler) where TEventData : IEventData
		{
		}

		/// <inheritdoc />
		public void Unregister(Type eventType, IEventHandler handler)
		{
		}

		/// <inheritdoc />
		public void Unregister<TEventData>(IEventHandlerFactory factory) where TEventData : IEventData
		{
		}

		/// <inheritdoc />
		public void Unregister(Type eventType, IEventHandlerFactory factory)
		{
		}

		/// <inheritdoc />
		public void UnregisterAll<TEventData>() where TEventData : IEventData
		{
		}

		/// <inheritdoc />
		public void UnregisterAll(Type @event)
		{
		}
        
		/// <inheritdoc />
		public void Publish<TEventData>(TEventData @event) where TEventData : IEventData
		{
		}

		/// <inheritdoc />
		public void Publish(Type eventType, IEventData @event)
        {
		}

		/// <inheritdoc />
		public Task PublishAsync<TEventData>(TEventData @event) where TEventData : IEventData
		{
			return new Task(() => { });
		}

		/// <inheritdoc />
		public Task PublishAsync<TEventData>(object eventSource, TEventData @event) where TEventData : IEventData
		{
			return new Task(() => { });
		}

		/// <inheritdoc />
		public Task PublishAsync(Type eventType, IEventData @event)
		{
			return new Task(() => { });
		}

		/// <inheritdoc />
		public Task PublishAsync(Type eventType, object eventSource, IEventData @event)
		{
			return new Task(() => { });
		}
	}
}
