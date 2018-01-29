using System.Threading;
using System.Threading.Tasks;

namespace Stove.Events.Bus.Entities
{
    /// <summary>
    ///     Null-object implementation of <see cref="IAggregateChangeEventHelper" />.
    /// </summary>
    public class NullAggregateChangeEventHelper : IAggregateChangeEventHelper
    {
        private NullAggregateChangeEventHelper()
        {
        }

        /// <summary>
        ///     Gets single instance of <see cref="NullEventBus" /> class.
        /// </summary>
        public static NullAggregateChangeEventHelper Instance { get; } = new NullAggregateChangeEventHelper();

        public void PublishEvents(AggregateChangeReport aggregateChangeReport)
        {
        }

        public Task PublishEventsAsync(AggregateChangeReport aggregateChangeReport, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(0);
        }
    }
}
