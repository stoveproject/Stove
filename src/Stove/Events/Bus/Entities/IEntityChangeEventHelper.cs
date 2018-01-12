using System.Threading;
using System.Threading.Tasks;

namespace Stove.Events.Bus.Entities
{
    /// <summary>
    ///     Used to publish entity change events.
    /// </summary>
    public interface IEntityChangeEventHelper
    {
        void PublishEvents(EntityChangeReport changeReport);

        Task PublishEventsAsync(EntityChangeReport changeReport, CancellationToken cancellationToken = default);
    }
}
