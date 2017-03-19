using Autofac.Extras.IocManager;

using Stove.Domain.Entities;
using Stove.Domain.Entities.Auditing;
using Stove.Extensions;
using Stove.Runtime.Session;
using Stove.Timing;

namespace Stove.Dapper.Filters.Action
{
    public class DeletionAuditDapperActionFilter : IDapperActionFilter, ITransientDependency
    {
        public DeletionAuditDapperActionFilter()
        {
            StoveSession = NullStoveSession.Instance;
        }

        public IStoveSession StoveSession { get; set; }

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
                long? userId = StoveSession.UserId;
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
