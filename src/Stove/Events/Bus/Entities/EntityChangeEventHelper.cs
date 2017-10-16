using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Autofac.Extras.IocManager;

using Stove.Domain.Uow;

namespace Stove.Events.Bus.Entities
{
    /// <summary>
    ///     Used to trigger entity change events.
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

        public virtual void TriggerEvents(EntityChangeReport changeReport)
        {
            TriggerEventsInternal(changeReport);

            if (changeReport.IsEmpty() || _unitOfWorkManager.Current == null) return;

            _unitOfWorkManager.Current.SaveChanges();
        }

        public Task TriggerEventsAsync(EntityChangeReport changeReport, CancellationToken cancellationToken = default(CancellationToken))
        {
            TriggerEventsInternal(changeReport);

            if (changeReport.IsEmpty() || _unitOfWorkManager.Current == null) return Task.FromResult(0);

            return _unitOfWorkManager.Current.SaveChangesAsync(cancellationToken);
        }

        public virtual void TriggerEntityCreatingEvent(object entity)
        {
            TriggerEventWithEntity(typeof(EntityCreatingEventData<>), entity, true);
        }

        public virtual void TriggerEntityCreatedEventOnUowCompleted(object entity)
        {
            TriggerEventWithEntity(typeof(EntityCreatedEventData<>), entity, false);
        }

        public virtual void TriggerEntityUpdatingEvent(object entity)
        {
            TriggerEventWithEntity(typeof(EntityUpdatingEventData<>), entity, true);
        }

        public virtual void TriggerEntityUpdatedEventOnUowCompleted(object entity)
        {
            TriggerEventWithEntity(typeof(EntityUpdatedEventData<>), entity, false);
        }

        public virtual void TriggerEntityDeletingEvent(object entity)
        {
            TriggerEventWithEntity(typeof(EntityDeletingEventData<>), entity, true);
        }

        public virtual void TriggerEntityDeletedEventOnUowCompleted(object entity)
        {
            TriggerEventWithEntity(typeof(EntityDeletedEventData<>), entity, false);
        }

        public virtual void TriggerEventsInternal(EntityChangeReport changeReport)
        {
            TriggerEntityChangeEvents(changeReport.ChangedEntities);
            TriggerDomainEvents(changeReport.DomainEvents);
        }

        protected virtual void TriggerEntityChangeEvents(List<EntityChangeEntry> changedEntities)
        {
            foreach (EntityChangeEntry changedEntity in changedEntities)
            {
                switch (changedEntity.ChangeType)
                {
                    case EntityChangeType.Created:
                        TriggerEntityCreatingEvent(changedEntity.Entity);
                        TriggerEntityCreatedEventOnUowCompleted(changedEntity.Entity);
                        break;
                    case EntityChangeType.Updated:
                        TriggerEntityUpdatingEvent(changedEntity.Entity);
                        TriggerEntityUpdatedEventOnUowCompleted(changedEntity.Entity);
                        break;
                    case EntityChangeType.Deleted:
                        TriggerEntityDeletingEvent(changedEntity.Entity);
                        TriggerEntityDeletedEventOnUowCompleted(changedEntity.Entity);
                        break;
                    default: throw new StoveException("Unknown EntityChangeType: " + changedEntity.ChangeType);
                }
            }
        }

        protected virtual void TriggerDomainEvents(List<DomainEventEntry> domainEvents)
        {
            foreach (DomainEventEntry domainEvent in domainEvents)
            {
                EventBus.Trigger(domainEvent.EventData.GetType(), domainEvent.SourceEntity, domainEvent.EventData);
            }
        }

        protected virtual void TriggerEventWithEntity(Type genericEventType, object entity, bool triggerInCurrentUnitOfWork)
        {
            Type entityType = entity.GetType();
            Type eventType = genericEventType.MakeGenericType(entityType);

            if (triggerInCurrentUnitOfWork || _unitOfWorkManager.Current == null)
            {
                EventBus.Trigger(eventType, (IEventData)Activator.CreateInstance(eventType, entity));
                return;
            }

            _unitOfWorkManager.Current.Completed += (sender, args) => EventBus.Trigger(eventType, (IEventData)Activator.CreateInstance(eventType, entity));
        }
    }
}
