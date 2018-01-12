using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Autofac.Extras.IocManager;

using Stove.Domain.Uow;

namespace Stove.Events.Bus.Entities
{
    /// <summary>
    ///     Used to publish entity change events.
    /// </summary>
    public class EntityChangeEventHelper : IEntityChangeEventHelper, ITransientDependency
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public EntityChangeEventHelper(IUnitOfWorkManager unitOfWorkManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
            EventBus = NullEventBus.Instance;
        }

        public IEventBus EventBus { get; set; }

        public virtual void PublishEvents(EntityChangeReport changeReport)
        {
            PublishEventsInternal(changeReport);

            if (changeReport.IsEmpty() || _unitOfWorkManager.Current == null)
            {
                return;
            }

            _unitOfWorkManager.Current.SaveChanges();
        }

        public Task PublishEventsAsync(EntityChangeReport changeReport, CancellationToken cancellationToken = default)
        {
            PublishEventsInternal(changeReport);

            if (changeReport.IsEmpty() || _unitOfWorkManager.Current == null)
            {
                return Task.FromResult(0);
            }

            return _unitOfWorkManager.Current.SaveChangesAsync(cancellationToken);
        }

        public virtual void PublishEventsInternal(EntityChangeReport changeReport)
        {
            PublishDomainEvents(changeReport.DomainEvents);
        }

        protected virtual void PublishDomainEvents(List<DomainEventEntry> domainEvents)
        {
            foreach (DomainEventEntry domainEvent in domainEvents)
            {
                EventBus.Publish(domainEvent.Event.GetType(), domainEvent.Event);
            }
        }
    }
}
