using System.Threading.Tasks;

namespace Stove.Events.Bus
{
	public class NullUnitOfWorkCompletedEventHelper : IUnitOfWorkCompletedEventHelper
	{
		public static NullUnitOfWorkCompletedEventHelper Instance { get; } = new NullUnitOfWorkCompletedEventHelper();

		public void Publish<T>(T @event) where T : IEvent
		{
		}

		public Task PublishAsync<T>(T @event) where T : IEvent
		{
			return null;
		}
	}
}
