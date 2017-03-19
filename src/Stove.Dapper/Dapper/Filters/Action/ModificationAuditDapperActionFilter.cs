using Autofac.Extras.IocManager;

using Stove.Domain.Entities;
using Stove.Domain.Entities.Auditing;
using Stove.Extensions;
using Stove.Runtime.Session;
using Stove.Timing;

namespace Stove.Dapper.Filters.Action
{
    public class ModificationAuditDapperActionFilter : IDapperActionFilter, ITransientDependency
    {
        public ModificationAuditDapperActionFilter()
        {
            StoveSession = NullStoveSession.Instance;
        }

        public IStoveSession StoveSession { get; set; }

        public void ExecuteFilter<TEntity, TPrimaryKey>(TEntity entity) where TEntity : class, IEntity<TPrimaryKey>
        {
            if (entity is IHasModificationTime)
            {
                entity.As<IHasModificationTime>().LastModificationTime = Clock.Now;
            }

            if (entity is IModificationAudited)
            {
                var record = entity.As<IModificationAudited>();
                long? userId = StoveSession.UserId;
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
