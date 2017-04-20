using Stove.Domain.Entities.Auditing;
using Stove.Extensions;
using Stove.Timing;

namespace Stove.Domain.Uow.DynamicFilters.Action
{
    public class ModificationAuditActionFilter : ActionFilterBase
    {
        public override void ExecuteFilter<TEntity, TPrimaryKey>(TEntity entity)
        {
            if (entity is IHasModificationTime)
            {
                entity.As<IHasModificationTime>().LastModificationTime = Clock.Now;
            }

            if (entity is IModificationAudited)
            {
                var record = entity.As<IModificationAudited>();
                long? userId = GetAuditUserId();
                if (userId == null)
                {
                    record.LastModifierUserId = null;
                    return;
                }

                record.LastModifierUserId = userId;
            }
        }
    }
}
