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

using Castle.Core.Internal;

using EntityFramework.DynamicFilters;

using Stove.Commands;
using Stove.Domain.Entities;
using Stove.Domain.Entities.Auditing;
using Stove.Domain.Uow;
using Stove.Events;
using Stove.Events.Bus;
using Stove.Events.Bus.Entities;
using Stove.Extensions;
using Stove.Log;
using Stove.Reflection;
using Stove.Runtime.Session;
using Stove.Timing;

namespace Stove.EntityFramework
{
    /// <inheritdoc cref="DbContext" />
    /// <summary>
    ///     Base class for all DbContext classes in the application.
    /// </summary>
    public abstract class StoveDbContext : DbContext, ITransientDependency, IStartable
    {
        /// <inheritdoc />
        /// <summary>
        ///     Constructor.
        ///     Uses <see cref="P:Stove.Configuration.IStoveStartupConfiguration.DefaultNameOrConnectionString" /> as connection
        ///     string.
        /// </summary>
        protected StoveDbContext()
        {
            InitializeDbContext();
        }

        /// <inheritdoc />
        /// <summary>
        ///     Constructor.
        /// </summary>
        protected StoveDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            InitializeDbContext();
        }

        /// <inheritdoc />
        /// <summary>
        ///     Constructor.
        /// </summary>
        protected StoveDbContext(DbCompiledModel model)
            : base(model)
        {
            InitializeDbContext();
        }

        /// <inheritdoc />
        /// <summary>
        ///     Constructor.
        /// </summary>
        protected StoveDbContext(DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection)
        {
            InitializeDbContext();
        }

        /// <inheritdoc />
        /// <summary>
        ///     Constructor.
        /// </summary>
        protected StoveDbContext(string nameOrConnectionString, DbCompiledModel model)
            : base(nameOrConnectionString, model)
        {
            InitializeDbContext();
        }

        /// <inheritdoc />
        /// <summary>
        ///     Constructor.
        /// </summary>
        protected StoveDbContext(ObjectContext objectContext, bool dbContextOwnsObjectContext)
            : base(objectContext, dbContextOwnsObjectContext)
        {
            InitializeDbContext();
        }

        /// <inheritdoc />
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
        ///     Used to pbulish entity change events.
        /// </summary>
        public IAggregateChangeEventHelper AggregateChangeEventHelper { get; set; }

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

        /// <summary>
        ///     Reference to command context.
        /// </summary>
        public IStoveCommandContextAccessor CommandContextAccessor { get; set; }

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
            AggregateChangeEventHelper = NullAggregateChangeEventHelper.Instance;
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
                AggregateChangeReport aggregateChangeReport = ApplyStoveConcepts();
                int result = base.SaveChanges();
                AggregateChangeEventHelper.PublishEvents(aggregateChangeReport);
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
                AggregateChangeReport aggregateChangeReport = ApplyStoveConcepts();
                int result = await base.SaveChangesAsync(cancellationToken);
                await AggregateChangeEventHelper.PublishEventsAsync(aggregateChangeReport, cancellationToken);
                return result;
            }
            catch (DbEntityValidationException ex)
            {
                LogDbEntityValidationException(ex);
                throw;
            }
        }

        protected virtual AggregateChangeReport ApplyStoveConcepts()
        {
            var changeReport = new AggregateChangeReport();

            long? userId = GetAuditUserId();
            List<DbEntityEntry> entries = ChangeTracker.Entries().ToList();
            foreach (DbEntityEntry entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        CheckAndSetId(entry.Entity);
                        SetCreationAuditProperties(entry.Entity, userId);
                        break;
                    case EntityState.Modified:
                        SetModificationAuditProperties(entry, userId);
                        if (entry.Entity is ISoftDelete && entry.Entity.As<ISoftDelete>().IsDeleted)
                        {
                            SetDeletionAuditProperties(entry.Entity, userId);
                        }
                        break;
                    case EntityState.Deleted:
                        CancelDeletionForSoftDelete(entry);
                        SetDeletionAuditProperties(entry.Entity, userId);
                        break;
                }

                AddDomainEvents(changeReport.DomainEvents, entry.Entity);
            }

            return changeReport;
        }

        protected virtual void AddDomainEvents(List<Envelope> domainEvents, object entity)
        {
            if (!(entity is IAggregateChangeTracker aggregateChangeTracker))
            {
                return;
            }

            if (aggregateChangeTracker.GetChanges().IsNullOrEmpty())
            {
                return;
            }

            domainEvents.AddRange(
                aggregateChangeTracker.GetChanges()
                                      .Select(@event => new Envelope(
                                          (IMessage)@event,
                                          new Headers()
                                          {
                                              [StoveConsts.Events.CausationId] = CommandContextAccessor.GetCorrelationIdOrEmpty(),
                                              [StoveConsts.Events.UserId] = StoveSession.UserId,
                                              [StoveConsts.Events.SourceType] = entity.GetType().FullName,
                                              [StoveConsts.Events.QualifiedName] = @event.GetType().AssemblyQualifiedName,
                                              [StoveConsts.Events.AggregateId] = ((dynamic)entity).Id
                                          })));

            aggregateChangeTracker.ClearChanges();
        }

        protected virtual void CheckAndSetId(object entityAsObj)
        {
            if (entityAsObj is IEntity<Guid> entity && entity.Id == Guid.Empty)
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
            EntityAuditingHelper.SetCreationAuditProperties(entityAsObj, userId);
        }

        protected virtual void SetModificationAuditProperties(DbEntityEntry entry, long? userId)
        {
            EntityAuditingHelper.SetModificationAuditProperties(entry.Entity, userId);
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
