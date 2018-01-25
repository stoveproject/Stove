using System.Threading;
using System.Threading.Tasks;

using Autofac.Extras.IocManager;

using Stove.Domain.Uow;

namespace Stove.Events.Bus.Entities
{
    /// <summary>
    ///     Used to publish aggregate change events.
    /// </summary>
    public class AggregateChangeEventHelper : IAggregateChangeEventHelper, ITransientDependency
    {
        private readonly IEventBus _eventBus;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public AggregateChangeEventHelper(IUnitOfWorkManager unitOfWorkManager, IEventBus eventBus)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _eventBus = eventBus;
        }

        public virtual void PublishEvents(AggregateChangeReport changeReport)
        {
            PublishEventsInternal(changeReport);

            if (changeReport.IsEmpty() || _unitOfWorkManager.Current == null)
            {
                return;
            }

            _unitOfWorkManager.Current.SaveChanges();
        }

        public Task PublishEventsAsync(AggregateChangeReport changeReport, CancellationToken cancellationToken = default)
        {
            PublishEventsInternal(changeReport);

            if (changeReport.IsEmpty() || _unitOfWorkManager.Current == null)
            {
                return Task.FromResult(0);
            }

            return _unitOfWorkManager.Current.SaveChangesAsync(cancellationToken);
        }

        protected virtual void PublishEventsInternal(AggregateChangeReport changeReport)
        {
            foreach (DomainEventEntry domainEvent in changeReport.DomainEvents)
            {
                _eventBus.Publish(domainEvent.Event.GetType(), domainEvent.Event);
            }
        }
    }
}
