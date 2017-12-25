namespace Stove.Events.Bus
{
    public interface IUnitOfWorkCompletedEventHelper
    {
        void Publish<T>(T @event) where T : IEvent;
    }
}
