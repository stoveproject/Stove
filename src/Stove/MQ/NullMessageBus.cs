using System;
using System.Threading.Tasks;

namespace Stove.MQ
{
	public class NullMessageBus : IMessageBus
	{
		public static readonly NullMessageBus Instance = new NullMessageBus();

	    public Task Publish<TMessage>(TMessage message) where TMessage : class
	    {
	        return null;
	    }

	    public Task Publish<TMessage>(object message) where TMessage : class
	    {
	        return null;
	    }

	    public Task Publish(object message)
	    {
	        return null;
	    }

	    public Task Publish(object message, Type messageType)
	    {
	        return null;
	    }

	    public Task<TResponse> CallRequest<TRequest, TResponse>(TRequest request, TimeSpan timeout, string queueName) where TRequest : class where TResponse : class
	    {
	        return Task.FromResult(default(TResponse));
        }

	    public Task<TResponse> CallRequest<TRequest, TResponse>(TRequest request, TimeSpan timeout, Uri queueUri) where TRequest : class where TResponse : class
	    {
	        return Task.FromResult(default(TResponse));
        }
	}
}
