using System.Threading;
using System.Threading.Tasks;

namespace Stove.Events.Bus.Entities
{
    /// <summary>
    ///     Used to publish entity change events.
    /// </summary>
    public interface IAggregateChangeEventHelper
    {
        void PublishEvents(AggregateChangeReport changeReport);

        Task PublishEventsAsync(AggregateChangeReport changeReport, CancellationToken cancellationToken = default);
    }
}
