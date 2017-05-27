using System.Threading.Tasks;

namespace Stove.Events.Bus
{
    public interface IUnitOfWorkCompletedEventHelper
    {
        void Trigger<T>(T @event) where T : IEventData;

        Task TriggerAsync<T>(T @event) where T : IEventData;
    }
}
