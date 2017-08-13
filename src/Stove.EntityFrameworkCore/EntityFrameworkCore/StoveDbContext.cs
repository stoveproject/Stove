using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Autofac.Extras.IocManager;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using Stove.Collections.Extensions;
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

namespace Stove.EntityFrameworkCore
{
	/// <summary>
	///     Base class for all DbContext classes in the application.
	/// </summary>
	public abstract class StoveDbContext : DbContext, ITransientDependency
	{
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

		/// <summary>
		///     Can be used to suppress automatically setting TenantId on SaveChanges.
		///     Default: false.
		/// </summary>
		public bool SuppressAutoSetTenantId { get; set; }

		private void InitializeDbContext()
		{
			SetNullsForInjectedProperties();
		}

		private void SetNullsForInjectedProperties()
		{
			Logger = NullLogger.Instance;
			StoveSession = NullStoveSession.Instance;
			EntityChangeEventHelper = NullEntityChangeEventHelper.Instance;
			GuidGenerator = SequentialGuidGenerator.Instance;
			EventBus = NullEventBus.Instance;
		}

		public override int SaveChanges()
		{
			try
			{
				EntityChangeReport changeReport = ApplyStoveConcepts();
				int result = base.SaveChanges();
				EntityChangeEventHelper.TriggerEvents(changeReport);
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
				EntityChangeReport changeReport = ApplyStoveConcepts();
				int result = await base.SaveChangesAsync(cancellationToken);
				await EntityChangeEventHelper.TriggerEventsAsync(changeReport);
				return result;
			}
			catch (DbUpdateConcurrencyException ex)
			{
				throw new StoveDbConcurrencyException(ex.Message, ex);
			}
		}

		protected virtual EntityChangeReport ApplyStoveConcepts()
		{
			var changeReport = new EntityChangeReport();

			long? userId = GetAuditUserId();

			foreach (EntityEntry entry in ChangeTracker.Entries().ToList())
			{
				ApplyStoveConcepts(entry, userId, changeReport);
			}

			return changeReport;
		}

		protected virtual void ApplyStoveConcepts(EntityEntry entry, long? userId, EntityChangeReport changeReport)
		{
			switch (entry.State)
			{
				case EntityState.Added:
					ApplyStoveConceptsForAddedEntity(entry, userId, changeReport);
					break;
				case EntityState.Modified:
					ApplyStoveConceptsForModifiedEntity(entry, userId, changeReport);
					break;
				case EntityState.Deleted:
					ApplyStoveConceptsForDeletedEntity(entry, userId, changeReport);
					break;
			}

			AddDomainEvents(changeReport.DomainEvents, entry.Entity);
		}

		protected virtual void ApplyStoveConceptsForAddedEntity(EntityEntry entry, long? userId, EntityChangeReport changeReport)
		{
			CheckAndSetId(entry);
			SetCreationAuditProperties(entry.Entity, userId);
			changeReport.ChangedEntities.Add(new EntityChangeEntry(entry.Entity, EntityChangeType.Created));
		}

		protected virtual void ApplyStoveConceptsForModifiedEntity(EntityEntry entry, long? userId, EntityChangeReport changeReport)
		{
			SetModificationAuditProperties(entry.Entity, userId);
			if (entry.Entity is ISoftDelete && entry.Entity.As<ISoftDelete>().IsDeleted)
			{
				SetDeletionAuditProperties(entry.Entity, userId);
				changeReport.ChangedEntities.Add(new EntityChangeEntry(entry.Entity, EntityChangeType.Deleted));
			}
			else
			{
				changeReport.ChangedEntities.Add(new EntityChangeEntry(entry.Entity, EntityChangeType.Updated));
			}
		}

		protected virtual void ApplyStoveConceptsForDeletedEntity(EntityEntry entry, long? userId, EntityChangeReport changeReport)
		{
			CancelDeletionForSoftDelete(entry);
			SetDeletionAuditProperties(entry.Entity, userId);
			changeReport.ChangedEntities.Add(new EntityChangeEntry(entry.Entity, EntityChangeType.Deleted));
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

		protected virtual void CheckAndSetId(EntityEntry entry)
		{
			//Set GUID Ids
			var entity = entry.Entity as IEntity<Guid>;
			if (entity != null && entity.Id == Guid.Empty)
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

				//Special check for multi-tenant entities

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
	}
}
