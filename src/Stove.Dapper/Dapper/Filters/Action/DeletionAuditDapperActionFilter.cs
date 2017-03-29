using Autofac.Extras.IocManager;

using Stove.Domain.Entities;
using Stove.Domain.Entities.Auditing;
using Stove.Extensions;
using Stove.Timing;

namespace Stove.Dapper.Filters.Action
{
    public class DeletionAuditDapperActionFilter : DapperActionFilterBase, IDapperActionFilter, ITransientDependency
    {
        public void ExecuteFilter<TEntity, TPrimaryKey>(TEntity entity) where TEntity : class, IEntity<TPrimaryKey>
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
