using Stove.Domain.Entities;
using Stove.Domain.Entities.Auditing;
using Stove.Extensions;
using Stove.Timing;

namespace Stove.Domain.Uow.DynamicFilters.Action
{
    public class DeletionAuditActionFilter : ActionFilterBase
    {
        public override void ExecuteFilter<TEntity, TPrimaryKey>(TEntity entity)
        {
            if (entity is ISoftDelete)
            {
                var record = entity.As<ISoftDelete>();
                record.IsDeleted = true;
            }

            if (entity is IHasDeletionTime)
            {
                var record = entity.As<IHasDeletionTime>();
                if (record.DeletionTime == null)
                {
                    record.DeletionTime = Clock.Now;
                }
            }

            if (entity is IDeletionAudited)
            {
                long? userId = GetAuditUserId();
                var record = entity.As<IDeletionAudited>();

                if (record.DeleterUserId != null)
                {
                    return;
                }

                if (userId == null)
                {
                    record.DeleterUserId = null;
                    return;
                }

                record.DeleterUserId = userId;
            }
        }
    }
}
