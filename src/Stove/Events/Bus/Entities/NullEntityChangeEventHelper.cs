using System.Threading;
using System.Threading.Tasks;

namespace Stove.Events.Bus.Entities
{
    /// <summary>
    ///     Null-object implementation of <see cref="IEntityChangeEventHelper" />.
    /// </summary>
    public class NullEntityChangeEventHelper : IEntityChangeEventHelper
    {
        private NullEntityChangeEventHelper()
        {
        }

        /// <summary>
        ///     Gets single instance of <see cref="NullEventBus" /> class.
        /// </summary>
        public static NullEntityChangeEventHelper Instance { get; } = new NullEntityChangeEventHelper();

        public void PublishEntityCreatingEvent(object entity)
        {
        }

        public void PublishEntityCreatedEventOnUowCompleted(object entity)
        {
        }

        public void PublishEntityUpdatingEvent(object entity)
        {
        }

        public void PublishEntityUpdatedEventOnUowCompleted(object entity)
        {
        }

        public void PublishEntityDeletingEvent(object entity)
        {
        }

        public void PublishEntityDeletedEventOnUowCompleted(object entity)
        {
        }

        public void PublishEvents(EntityChangeReport changeReport)
        {
        }

        public Task PublishEventsAsync(EntityChangeReport changeReport, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(0);
        }
    }
}
