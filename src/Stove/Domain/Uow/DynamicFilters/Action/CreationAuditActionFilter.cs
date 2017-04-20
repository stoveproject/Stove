using System;

using Stove.Domain.Entities.Auditing;
using Stove.Extensions;
using Stove.Timing;

namespace Stove.Domain.Uow.DynamicFilters.Action
{
    public class CreationAuditActionFilter : ActionFilterBase
    {
        public override void ExecuteFilter<TEntity, TPrimaryKey>(TEntity entity)
        {
            long? userId = GetAuditUserId();
            CheckAndSetId(entity);
            var entityWithCreationTime = entity as IHasCreationTime;
            if (entityWithCreationTime == null)
            {
                return;
            }

            if (entityWithCreationTime.CreationTime == default(DateTime))
            {
                entityWithCreationTime.CreationTime = Clock.Now;
            }

            if (userId.HasValue && entity is ICreationAudited)
            {
                var record = entity as ICreationAudited;
                if (record.CreatorUserId == null)
                {
                    record.CreatorUserId = userId;
                }
            }

            if (entity is IHasModificationTime)
            {
                entity.As<IHasModificationTime>().LastModificationTime = null;
            }

            if (entity is IModificationAudited)
            {
                var record = entity.As<IModificationAudited>();
                record.LastModifierUserId = null;
            }
        }
    }
}
