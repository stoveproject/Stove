using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Autofac.Extras.IocManager;

using Castle.Core.Internal;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

using Stove.Collections.Extensions;
using Stove.Domain.Entities;
using Stove.Domain.Entities.Auditing;
using Stove.Domain.Uow;
using Stove.Events;
using Stove.Events.Bus;
using Stove.Events.Bus.Entities;
using Stove.Extensions;
using Stove.Linq.Expressions;
using Stove.Log;
using Stove.Reflection;
using Stove.Runtime.Session;
using Stove.Timing;

namespace Stove.EntityFrameworkCore
{
    /// <inheritdoc cref="DbContext" />
    /// <summary>
    ///     Base class for all DbContext classes in the application.
    /// </summary>
    public abstract class StoveDbContext : DbContext, ITransientDependency
    {
        private static readonly MethodInfo ConfigureGlobalFiltersMethodInfo = typeof(StoveDbContext).GetMethod(nameof(ConfigureGlobalFilters), BindingFlags.Instance | BindingFlags.NonPublic);

        /// <inheritdoc />
        /// <summary>
        ///     Constructor.
        /// </summary>
        protected StoveDbContext(DbContextOptions options)
            : base(options)
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

        protected virtual bool IsSoftDeleteFilterEnabled => CurrentUnitOfWorkProvider?.Current?.IsFilterEnabled(StoveDataFilters.SoftDelete) == true;

        private void InitializeDbContext()
        {
            SetNullsForInjectedProperties();
        }

        private void SetNullsForInjectedProperties()
        {
            Logger = NullLogger.Instance;
            StoveSession = NullStoveSession.Instance;
            AggregateChangeEventHelper = NullAggregateChangeEventHelper.Instance;
            GuidGenerator = SequentialGuidGenerator.Instance;
            EventBus = NullEventBus.Instance;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
            {
                ConfigureGlobalFiltersMethodInfo
                    .MakeGenericMethod(entityType.ClrType)
                    .Invoke(this, new object[] { modelBuilder, entityType });
            }
        }

        public override int SaveChanges()
        {
            try
            {
                AggregateChangeReport changeReport = ApplyStoveConcepts();
                int result = base.SaveChanges();
                AggregateChangeEventHelper.PublishEvents(changeReport);
                return result;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new StoveDbConcurrencyException(ex.Message, ex);
            }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                AggregateChangeReport changeReport = ApplyStoveConcepts();
                int result = await base.SaveChangesAsync(cancellationToken);
                await AggregateChangeEventHelper.PublishEventsAsync(changeReport, cancellationToken);
                return result;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new StoveDbConcurrencyException(ex.Message, ex);
            }
        }

        protected virtual AggregateChangeReport ApplyStoveConcepts()
        {
            var changeReport = new AggregateChangeReport();

            long? userId = GetAuditUserId();

            if (ChangeTracker.HasChanges())
            {
                foreach (EntityEntry entry in ChangeTracker.Entries().ToList())
                {
                    ApplyStoveConcepts(entry, userId, changeReport);
                }
            }

            return changeReport;
        }

        protected virtual void ApplyStoveConcepts(EntityEntry entry, long? userId, AggregateChangeReport changeReport)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    ApplyStoveConceptsForAddedEntity(entry, userId);
                    break;
                case EntityState.Modified:
                    ApplyStoveConceptsForModifiedEntity(entry, userId);
                    break;
                case EntityState.Deleted:
                    ApplyStoveConceptsForDeletedEntity(entry, userId);
                    break;
            }

            AddDomainEvents(changeReport.DomainEvents, entry.Entity);
        }

        protected virtual void ApplyStoveConceptsForAddedEntity(EntityEntry entry, long? userId)
        {
            CheckAndSetId(entry);
            SetCreationAuditProperties(entry.Entity, userId);
        }

        protected virtual void ApplyStoveConceptsForModifiedEntity(EntityEntry entry, long? userId)
        {
            SetModificationAuditProperties(entry.Entity, userId);
            if (entry.Entity is ISoftDelete && entry.Entity.As<ISoftDelete>().IsDeleted)
            {
                SetDeletionAuditProperties(entry.Entity, userId);
            }
        }

        protected virtual void ApplyStoveConceptsForDeletedEntity(EntityEntry entry, long? userId)
        {
            CancelDeletionForSoftDelete(entry);
            SetDeletionAuditProperties(entry.Entity, userId);
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
                                          (IEvent)@event,
                                          new EventHeaders()
                                          {
                                              [StoveConsts.Events.CausationId] = CommandContextAccessor.GetCorrelationIdOrEmpty(),
                                              [StoveConsts.Events.UserId] = StoveSession.UserId,
                                              [StoveConsts.Events.SourceType] = entity.GetType().FullName,
                                              [StoveConsts.Events.QualifiedName] = @event.GetType().AssemblyQualifiedName,
                                              [StoveConsts.Events.AggregateId] = ((dynamic)entity).Id
                                          })));

            aggregateChangeTracker.ClearChanges();
        }

        protected virtual void CheckAndSetId(EntityEntry entry)
        {
            if (entry.Entity is IEntity<Guid> entity && entity.Id == Guid.Empty)
            {
                var dbGeneratedAttr = ReflectionHelper
                    .GetSingleAttributeOrDefault<DatabaseGeneratedAttribute>(
                        entry.Property("Id").Metadata.PropertyInfo
                    );

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

        protected virtual void SetModificationAuditProperties(object entityAsObj, long? userId)
        {
            EntityAuditingHelper.SetModificationAuditProperties(entityAsObj, userId);
        }

        protected virtual void CancelDeletionForSoftDelete(EntityEntry entry)
        {
            if (!(entry.Entity is ISoftDelete))
            {
                return;
            }

            entry.Reload();
            entry.State = EntityState.Modified;
            entry.Entity.As<ISoftDelete>().IsDeleted = true;
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

        protected virtual bool ShouldFilterEntity<TEntity>(IMutableEntityType entityType) where TEntity : class
        {
            if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
            {
                return true;
            }

            return false;
        }

        protected void ConfigureGlobalFilters<TEntity>(ModelBuilder modelBuilder, IMutableEntityType entityType)
            where TEntity : class
        {
            if (entityType.BaseType == null && ShouldFilterEntity<TEntity>(entityType))
            {
                Expression<Func<TEntity, bool>> filterExpression = CreateFilterExpression<TEntity>();
                if (filterExpression != null)
                {
                    modelBuilder.Entity<TEntity>().HasQueryFilter(filterExpression);
                }
            }
        }

        protected virtual Expression<Func<TEntity, bool>> CreateFilterExpression<TEntity>()
            where TEntity : class
        {
            Expression<Func<TEntity, bool>> expression = null;

            if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
            {
                /* This condition should normally be defined as below:
                 * !IsSoftDeleteFilterEnabled || !((ISoftDelete) e).IsDeleted
                 * But this causes a problem with EF Core (see https://github.com/aspnet/EntityFrameworkCore/issues/9502)
                 * So, we made a workaround to make it working. It works same as above.
                 */

                Expression<Func<TEntity, bool>> softDeleteFilter = e => !((ISoftDelete)e).IsDeleted || ((ISoftDelete)e).IsDeleted != IsSoftDeleteFilterEnabled;
                expression = expression == null ? softDeleteFilter : expression.And(softDeleteFilter);
            }

            return expression;
        }
    }
}
