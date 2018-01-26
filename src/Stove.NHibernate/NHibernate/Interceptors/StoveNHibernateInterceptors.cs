using System;
using System.Collections.Generic;
using System.Linq;

using Autofac.Extras.IocManager;

using Castle.Core.Internal;

using NHibernate;
using NHibernate.Type;

using Stove.Domain.Entities;
using Stove.Domain.Entities.Auditing;
using Stove.Events.Bus;
using Stove.Extensions;
using Stove.Runtime.Session;
using Stove.Timing;

namespace Stove.NHibernate.Interceptors
{
    internal class StoveNHibernateInterceptor : EmptyInterceptor, ITransientDependency
    {
        private readonly Lazy<IEventBus> _eventBus;
        private readonly Lazy<IGuidGenerator> _guidGenerator;

        private readonly IScopeResolver _scopeResolver;
        private readonly Lazy<IStoveSession> _stoveSession;

        public StoveNHibernateInterceptor(IScopeResolver scopeResolver)
        {
            _scopeResolver = scopeResolver;

            _stoveSession =
                new Lazy<IStoveSession>(
                    () => _scopeResolver.IsRegistered(typeof(IStoveSession))
                        ? _scopeResolver.Resolve<IStoveSession>()
                        : NullStoveSession.Instance,
                    true
                );
            _guidGenerator =
                new Lazy<IGuidGenerator>(
                    () => _scopeResolver.IsRegistered(typeof(IGuidGenerator))
                        ? _scopeResolver.Resolve<IGuidGenerator>()
                        : SequentialGuidGenerator.Instance,
                    true
                );

            _eventBus =
                new Lazy<IEventBus>(
                    () => _scopeResolver.IsRegistered(typeof(IEventBus))
                        ? _scopeResolver.Resolve<IEventBus>()
                        : NullEventBus.Instance,
                    true
                );
        }

        public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, IType[] types)
        {
            //Set Id for Guids
            if (entity is IEntity<Guid>)
            {
                var guidEntity = entity as IEntity<Guid>;
                if (guidEntity.IsTransient())
                {
                    guidEntity.Id = _guidGenerator.Value.Create();
                }
            }

            //Set CreationTime for new entity
            if (entity is IHasCreationTime)
            {
                for (var i = 0; i < propertyNames.Length; i++)
                {
                    if (propertyNames[i] == "CreationTime")
                    {
                        state[i] = (entity as IHasCreationTime).CreationTime = Clock.Now;
                    }
                }
            }

            //Set CreatorUserId for new entity
            if (entity is ICreationAudited && _stoveSession.Value.UserId.HasValue)
            {
                for (var i = 0; i < propertyNames.Length; i++)
                {
                    if (propertyNames[i] == "CreatorUserId")
                    {
                        state[i] = (entity as ICreationAudited).CreatorUserId = _stoveSession.Value.UserId;
                    }
                }
            }

            TriggerDomainEvents(entity);

            return base.OnSave(entity, id, state, propertyNames, types);
        }

        public override bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames, IType[] types)
        {
            //Set modification audits
            if (entity is IHasModificationTime)
            {
                for (var i = 0; i < propertyNames.Length; i++)
                {
                    if (propertyNames[i] == "LastModificationTime")
                    {
                        currentState[i] = (entity as IHasModificationTime).LastModificationTime = Clock.Now;
                    }
                }
            }

            if (entity is IModificationAudited && _stoveSession.Value.UserId.HasValue)
            {
                for (var i = 0; i < propertyNames.Length; i++)
                {
                    if (propertyNames[i] == "LastModifierUserId")
                    {
                        currentState[i] = (entity as IModificationAudited).LastModifierUserId = _stoveSession.Value.UserId;
                    }
                }
            }

            if (entity is ISoftDelete && entity.As<ISoftDelete>().IsDeleted)
            {
                //Is deleted before? Normally, a deleted entity should not be updated later but I preferred to check it.
                var previousIsDeleted = false;
                for (var i = 0; i < propertyNames.Length; i++)
                {
                    if (propertyNames[i] == "IsDeleted")
                    {
                        previousIsDeleted = (bool)previousState[i];
                        break;
                    }
                }

                if (!previousIsDeleted)
                {
                    //set DeletionTime
                    if (entity is IHasDeletionTime)
                    {
                        for (var i = 0; i < propertyNames.Length; i++)
                        {
                            if (propertyNames[i] == "DeletionTime")
                            {
                                currentState[i] = (entity as IHasDeletionTime).DeletionTime = Clock.Now;
                            }
                        }
                    }

                    //set DeleterUserId
                    if (entity is IDeletionAudited && _stoveSession.Value.UserId.HasValue)
                    {
                        for (var i = 0; i < propertyNames.Length; i++)
                        {
                            if (propertyNames[i] == "DeleterUserId")
                            {
                                currentState[i] = (entity as IDeletionAudited).DeleterUserId = _stoveSession.Value.UserId;
                            }
                        }
                    }

                   
                }
            }

            TriggerDomainEvents(entity);

            return base.OnFlushDirty(entity, id, currentState, previousState, propertyNames, types);
        }

        public override void OnDelete(object entity, object id, object[] state, string[] propertyNames, IType[] types)
        {
            TriggerDomainEvents(entity);

            base.OnDelete(entity, id, state, propertyNames, types);
        }

        public override bool OnLoad(object entity, object id, object[] state, string[] propertyNames, IType[] types)
        {
            NormalizeDateTimePropertiesForEntity(state, types);
            return true;
        }

        private static void NormalizeDateTimePropertiesForEntity(object[] state, IList<IType> types)
        {
            for (var i = 0; i < types.Count; i++)
            {
                if (types[i].IsComponentType)
                {
                    NormalizeDateTimePropertiesForComponentType(state[i], types[i]);
                }

                if (types[i].ReturnedClass != typeof(DateTime) && types[i].ReturnedClass != typeof(DateTime?))
                {
                    continue;
                }

                if (!(state[i] is DateTime dateTime))
                {
                    continue;
                }

                state[i] = Clock.Normalize(dateTime);
            }
        }

        private static void NormalizeDateTimePropertiesForComponentType(object componentObject, IType type)
        {
            if (type is ComponentType componentType)
            {
                for (var i = 0; i < componentType.PropertyNames.Length; i++)
                {
                    string propertyName = componentType.PropertyNames[i];
                    if (componentType.Subtypes[i].IsComponentType)
                    {
                        object value = componentObject.GetType().GetProperty(propertyName)?.GetValue(componentObject, null);
                        NormalizeDateTimePropertiesForComponentType(value, componentType.Subtypes[i]);
                    }

                    if (componentType.Subtypes[i].ReturnedClass != typeof(DateTime) && componentType.Subtypes[i].ReturnedClass != typeof(DateTime?))
                    {
                        continue;
                    }

                    var dateTime = componentObject.GetType().GetProperty(propertyName)?.GetValue(componentObject) as DateTime?;

                    if (!dateTime.HasValue)
                    {
                        continue;
                    }

                    componentObject.GetType().GetProperty(propertyName)?.SetValue(componentObject, Clock.Normalize(dateTime.Value));
                }
            }
        }

        protected virtual void TriggerDomainEvents(object entityAsObj)
        {
            if (!(entityAsObj is IAggregateChangeTracker aggregateChangeTracker))
            {
                return;
            }

            if (aggregateChangeTracker.GetChanges().IsNullOrEmpty())
            {
                return;
            }

            List<object> domainEvents = aggregateChangeTracker.GetChanges().ToList();
            aggregateChangeTracker.ClearChanges();

            foreach (object domainEvent in domainEvents)
            {
                _eventBus.Value.Publish(domainEvent.GetType(), (IEvent)domainEvent);
            }
        }
    }
}
