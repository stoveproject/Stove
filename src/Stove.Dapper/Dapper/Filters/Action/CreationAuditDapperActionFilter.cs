using System;

using Autofac.Extras.IocManager;

using Stove.Domain.Entities;
using Stove.Domain.Entities.Auditing;
using Stove.Extensions;
using Stove.Runtime.Session;
using Stove.Timing;

namespace Stove.Dapper.Filters.Action
{
    public class CreationAuditDapperActionFilter : IDapperActionFilter, ITransientDependency
    {
        public CreationAuditDapperActionFilter()
        {
            StoveSession = NullStoveSession.Instance;
        }

        public IStoveSession StoveSession { get; set; }

        public void ExecuteFilter<TEntity, TPrimaryKey>(TEntity entity) where TEntity : class, IEntity<TPrimaryKey>
        {
            long? userId = StoveSession.UserId;
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
