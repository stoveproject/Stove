using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Autofac;
using Autofac.Extras.IocManager;

using EntityFramework.DynamicFilters;

using Stove.Collections.Extensions;
using Stove.Configuration;
using Stove.Domain.Entities;
using Stove.Domain.Entities.Auditing;
using Stove.Domain.Uow;
using Stove.Events.Bus;
using Stove.Events.Bus.Entities;
using Stove.Extensions;
using Stove.Log;
using Stove.Reflection;
using Stove.Runtime.Session;
using Stove.Timing;

namespace Stove.EntityFramework
{
    /// <summary>
    ///     Base class for all DbContext classes in the application.
    /// </summary>
    public abstract class StoveDbContext : DbContext, ITransientDependency, IStartable
    {
        /// <summary>
        ///     Constructor.
        ///     Uses <see cref="IStoveStartupConfiguration.DefaultNameOrConnectionString" /> as connection string.
        /// </summary>
        protected StoveDbContext()
        {
            InitializeDbContext();
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        protected StoveDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            InitializeDbContext();
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        protected StoveDbContext(DbCompiledModel model)
            : base(model)
        {
            InitializeDbContext();
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        protected StoveDbContext(DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection)
        {
            InitializeDbContext();
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        protected StoveDbContext(string nameOrConnectionString, DbCompiledModel model)
            : base(nameOrConnectionString, model)
        {
            InitializeDbContext();
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        protected StoveDbContext(ObjectContext objectContext, bool dbContextOwnsObjectContext)
            : base(objectContext, dbContextOwnsObjectContext)
        {
            InitializeDbContext();
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        protected StoveDbContext(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection)
            : base(existingConnection, model, contextOwnsConnection)
        {
            InitializeDbContext();
        }

        /// <summary>
        ///     Used to get current session values.
        /// </summary>
        public IStoveSession StoveSession { get; set; }

        /// <summary>
        ///     Used to trigger entity change events.
        /// </summary>
        public IEntityChangeEventHelper EntityChangeEventHelper { get; set; }

        /// <summary>
        ///     Reference to the logger.
        /// </summary>
        public ILogger Logger { get; set; }

        /// <summary>
        ///     Reference to the event bus.
        /// </summary>
        public IEventBus EventBus { get; set; }

        /// <summary>
        ///     Reference to GUID generator.
        /// </summary>
        public IGuidGenerator GuidGenerator { get; set; }

        /// <summary>
        ///     Reference to the current UOW provider.
        /// </summary>
        public ICurrentUnitOfWorkProvider CurrentUnitOfWorkProvider { get; set; }

        public virtual void Start()
        {
            Database.Initialize(false);
        }

        private void InitializeDbContext()
        {
            SetNullsForInjectedProperties();
            RegisterToChanges();
        }

        private void RegisterToChanges()
        {
            ((IObjectContextAdapter)this)
                .ObjectContext
                .ObjectStateManager
                .ObjectStateManagerChanged += ObjectStateManager_ObjectStateManagerChanged;
        }

        protected virtual void ObjectStateManager_ObjectStateManagerChanged(object sender, CollectionChangeEventArgs e)
        {
            var contextAdapter = (IObjectContextAdapter)this;
            if (e.Action != CollectionChangeAction.Add)
            {
                return;
            }

            ObjectStateEntry entry = contextAdapter.ObjectContext.ObjectStateManager.GetObjectStateEntry(e.Element);
            switch (entry.State)
            {
                case EntityState.Added:
                    CheckAndSetId(entry.Entity);
                    SetCreationAuditProperties(entry.Entity, GetAuditUserId());
                    break;
            }
        }

        private void SetNullsForInjectedProperties()
        {
            Logger = NullLogger.Instance;
            StoveSession = NullStoveSession.Instance;
            EntityChangeEventHelper = NullEntityChangeEventHelper.Instance;
            GuidGenerator = SequentialGuidGenerator.Instance;
            EventBus = NullEventBus.Instance;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Filter(StoveDataFilters.SoftDelete, (ISoftDelete d) => d.IsDeleted, false);
        }

        public override int SaveChanges()
        {
            try
            {
                EntityChangeReport changedEntities = ApplyStoveConcepts();
                int result = base.SaveChanges();
                EntityChangeEventHelper.TriggerEvents(changedEntities);
                return result;
            }
            catch (DbEntityValidationException ex)
            {
                LogDbEntityValidationException(ex);
                throw;
            }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            try
            {
                EntityChangeReport changeReport = ApplyStoveConcepts();
                int result = await base.SaveChangesAsync(cancellationToken);
                await EntityChangeEventHelper.TriggerEventsAsync(changeReport);
                return result;
            }
            catch (DbEntityValidationException ex)
            {
                LogDbEntityValidationException(ex);
                throw;
            }
        }

        protected virtual EntityChangeReport ApplyStoveConcepts()
        {
            var changeReport = new EntityChangeReport();

            long? userId = GetAuditUserId();

            List<DbEntityEntry> entries = ChangeTracker.Entries().ToList();
            foreach (DbEntityEntry entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        CheckAndSetId(entry.Entity);
                        SetCreationAuditProperties(entry.Entity, userId);
                        changeReport.ChangedEntities.Add(new EntityChangeEntry(entry.Entity, EntityChangeType.Created));
                        break;
                    case EntityState.Modified:
                        SetModificationAuditProperties(entry, userId);
                        if (entry.Entity is ISoftDelete && entry.Entity.As<ISoftDelete>().IsDeleted)
                        {
                            SetDeletionAuditProperties(entry.Entity, userId);
                            changeReport.ChangedEntities.Add(new EntityChangeEntry(entry.Entity, EntityChangeType.Deleted));
                        }
                        else
                        {
                            changeReport.ChangedEntities.Add(new EntityChangeEntry(entry.Entity, EntityChangeType.Updated));
                        }

                        break;
                    case EntityState.Deleted:
                        CancelDeletionForSoftDelete(entry);
                        SetDeletionAuditProperties(entry.Entity, userId);
                        changeReport.ChangedEntities.Add(new EntityChangeEntry(entry.Entity, EntityChangeType.Deleted));
                        break;
                }

                AddDomainEvents(changeReport.DomainEvents, entry.Entity);
            }

            return changeReport;
        }

        protected virtual void AddDomainEvents(List<DomainEventEntry> domainEvents, object entityAsObj)
        {
            var generatesDomainEventsEntity = entityAsObj as IGeneratesDomainEvents;
            if (generatesDomainEventsEntity == null)
            {
                return;
            }

            if (generatesDomainEventsEntity.DomainEvents.IsNullOrEmpty())
            {
                return;
            }

            domainEvents.AddRange(generatesDomainEventsEntity.DomainEvents.Select(eventData => new DomainEventEntry(entityAsObj, eventData)));
            generatesDomainEventsEntity.DomainEvents.Clear();
        }

        protected virtual void CheckAndSetId(object entityAsObj)
        {
            //Set GUID Ids
            var entity = entityAsObj as IEntity<Guid>;
            if (entity != null && entity.Id == Guid.Empty)
            {
                Type entityType = ObjectContext.GetObjectType(entityAsObj.GetType());
                PropertyInfo idProperty = entityType.GetProperty("Id");
                var dbGeneratedAttr = ReflectionHelper.GetSingleAttributeOrDefault<DatabaseGeneratedAttribute>(idProperty);
                if (dbGeneratedAttr == null || dbGeneratedAttr.DatabaseGeneratedOption == DatabaseGeneratedOption.None)
                {
                    entity.Id = GuidGenerator.Create();
                }
            }
        }

        protected virtual void SetCreationAuditProperties(object entityAsObj, long? userId)
        {
            var entityWithCreationTime = entityAsObj as IHasCreationTime;
            if (entityWithCreationTime == null)
            {
                return;
            }

            if (entityWithCreationTime.CreationTime == default(DateTime))
            {
                entityWithCreationTime.CreationTime = Clock.Now;
            }

            if (userId.HasValue && entityAsObj is ICreationAudited)
            {
                var entity = entityAsObj as ICreationAudited;
                if (entity.CreatorUserId == null)
                {
                    entity.CreatorUserId = userId;
                }
            }
        }

        protected virtual void SetModificationAuditProperties(DbEntityEntry entry, long? userId)
        {
            if (entry.Entity is IHasModificationTime)
            {
                entry.Cast<IHasModificationTime>().Entity.LastModificationTime = Clock.Now;
            }

            if (entry.Entity is IModificationAudited)
            {
                IModificationAudited entity = entry.Cast<IModificationAudited>().Entity;

                if (userId == null)
                {
                    entity.LastModifierUserId = null;
                    return;
                }

                entity.LastModifierUserId = userId;
            }
        }

        protected virtual void CancelDeletionForSoftDelete(DbEntityEntry entry)
        {
            if (!(entry.Entity is ISoftDelete))
            {
                return;
            }

            DbEntityEntry<ISoftDelete> softDeleteEntry = entry.Cast<ISoftDelete>();
            softDeleteEntry.Reload();
            softDeleteEntry.State = EntityState.Modified;
            softDeleteEntry.Entity.IsDeleted = true;
        }

        protected virtual void SetDeletionAuditProperties(object entityAsObj, long? userId)
        {
            if (entityAsObj is IHasDeletionTime)
            {
                var entity = entityAsObj.As<IHasDeletionTime>();

                if (entity.DeletionTime == null)
                {
                    entity.DeletionTime = Clock.Now;
                }
            }

            if (entityAsObj is IDeletionAudited)
            {
                var entity = entityAsObj.As<IDeletionAudited>();

                if (entity.DeleterUserId != null)
                {
                    return;
                }

                if (userId == null)
                {
                    entity.DeleterUserId = null;
                    return;
                }

                entity.DeleterUserId = userId;
            }
        }

        protected virtual void LogDbEntityValidationException(DbEntityValidationException exception)
        {
            Logger.Error("There are some validation errors while saving changes in EntityFramework:");
            foreach (DbValidationError ve in exception.EntityValidationErrors.SelectMany(eve => eve.ValidationErrors))
            {
                Logger.Error(" - " + ve.PropertyName + ": " + ve.ErrorMessage);
            }
        }

        protected virtual long? GetAuditUserId()
        {
            if (StoveSession.UserId.HasValue &&
                CurrentUnitOfWorkProvider != null &&
                CurrentUnitOfWorkProvider.Current != null)
            {
                return StoveSession.UserId;
            }

            return null;
        }
    }
}
