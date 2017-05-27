using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Stove.Events.Bus
{
    [ExcludeFromCodeCoverage]
    public class NullUnitOfWorkCompletedEventHelper : IUnitOfWorkCompletedEventHelper
    {
        public static NullUnitOfWorkCompletedEventHelper Instance { get; } = new NullUnitOfWorkCompletedEventHelper();

        public void Trigger<T>(T @event) where T : IEventData
        {
        }

        public Task TriggerAsync<T>(T @event) where T : IEventData
        {
            return null;
        }
    }
}
