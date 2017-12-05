using System;
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

            if (changeReport.IsEmpty() || _unitOfWorkManager.Current == null) return;

            _unitOfWorkManager.Current.SaveChanges();
        }

        public Task PublishEventsAsync(EntityChangeReport changeReport, CancellationToken cancellationToken = default(CancellationToken))
        {
            PublishEventsInternal(changeReport);

            if (changeReport.IsEmpty() || _unitOfWorkManager.Current == null) return Task.FromResult(0);

            return _unitOfWorkManager.Current.SaveChangesAsync(cancellationToken);
        }

        public virtual void PublishEntityCreatingEvent(object entity)
        {
            PublishEventWithEntity(typeof(EntityCreatingEventData<>), entity, true);
        }

        public virtual void PublishEntityCreatedEventOnUowCompleted(object entity)
        {
            PublishEventWithEntity(typeof(EntityCreatedEventData<>), entity, false);
        }

        public virtual void PublishEntityUpdatingEvent(object entity)
        {
            PublishEventWithEntity(typeof(EntityUpdatingEventData<>), entity, true);
        }

        public virtual void PublishEntityUpdatedEventOnUowCompleted(object entity)
        {
            PublishEventWithEntity(typeof(EntityUpdatedEventData<>), entity, false);
        }

        public virtual void PublishEntityDeletingEvent(object entity)
        {
            PublishEventWithEntity(typeof(EntityDeletingEventData<>), entity, true);
        }

        public virtual void PublishEntityDeletedEventOnUowCompleted(object entity)
        {
            PublishEventWithEntity(typeof(EntityDeletedEventData<>), entity, false);
        }

        public virtual void PublishEventsInternal(EntityChangeReport changeReport)
        {
            PublishEntityChangeEvents(changeReport.ChangedEntities);
            PublishDomainEvents(changeReport.DomainEvents);
        }

        protected virtual void PublishEntityChangeEvents(List<EntityChangeEntry> changedEntities)
        {
            foreach (EntityChangeEntry changedEntity in changedEntities)
            {
                switch (changedEntity.ChangeType)
                {
                    case EntityChangeType.Created:
                        PublishEntityCreatingEvent(changedEntity.Entity);
                        PublishEntityCreatedEventOnUowCompleted(changedEntity.Entity);
                        break;
                    case EntityChangeType.Updated:
                        PublishEntityUpdatingEvent(changedEntity.Entity);
                        PublishEntityUpdatedEventOnUowCompleted(changedEntity.Entity);
                        break;
                    case EntityChangeType.Deleted:
                        PublishEntityDeletingEvent(changedEntity.Entity);
                        PublishEntityDeletedEventOnUowCompleted(changedEntity.Entity);
                        break;
                    default: throw new StoveException("Unknown EntityChangeType: " + changedEntity.ChangeType);
                }
            }
        }

        protected virtual void PublishDomainEvents(List<DomainEventEntry> domainEvents)
        {
            foreach (DomainEventEntry domainEvent in domainEvents)
            {
                EventBus.Publish(domainEvent.EventData.GetType(), domainEvent.EventData);
            }
        }

        protected virtual void PublishEventWithEntity(Type genericEventType, object entity, bool publishInCurrentUnitOfWork)
        {
            Type entityType = entity.GetType();
            Type eventType = genericEventType.MakeGenericType(entityType);

            if (publishInCurrentUnitOfWork || _unitOfWorkManager.Current == null)
            {
                EventBus.Publish(eventType, (IEventData)Activator.CreateInstance(eventType, entity));
                return;
            }

            _unitOfWorkManager.Current.Completed += (sender, args) => EventBus.Publish(eventType, (IEventData)Activator.CreateInstance(eventType, entity));
        }
    }
}
