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

        Task PublishEventsAsync(EntityChangeReport changeReport, CancellationToken cancellationToken = default(CancellationToken));

        void PublishEntityCreatingEvent(object entity);

        void PublishEntityCreatedEventOnUowCompleted(object entity);

        void PublishEntityUpdatingEvent(object entity);

        void PublishEntityUpdatedEventOnUowCompleted(object entity);

        void PublishEntityDeletingEvent(object entity);

        void PublishEntityDeletedEventOnUowCompleted(object entity);
    }
}
