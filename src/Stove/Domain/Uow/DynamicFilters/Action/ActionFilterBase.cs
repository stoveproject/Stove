using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

using Autofac.Extras.IocManager;

using Stove.Domain.Entities;
using Stove.Reflection;
using Stove.Runtime.Session;

namespace Stove.Domain.Uow.DynamicFilters.Action
{
    public abstract class ActionFilterBase : ITransientDependency
    {
        protected ActionFilterBase()
        {
            StoveSession = NullStoveSession.Instance;
            GuidGenerator = SequentialGuidGenerator.Instance;
        }

        public IStoveSession StoveSession { get; set; }

        public ICurrentUnitOfWorkProvider CurrentUnitOfWorkProvider { get; set; }

        public IGuidGenerator GuidGenerator { get; set; }

        protected virtual long? GetAuditUserId()
        {
            if (StoveSession.UserId.HasValue && CurrentUnitOfWorkProvider?.Current != null)
            {
                return StoveSession.UserId;
            }

            return null;
        }

        public abstract void ExecuteFilter<TEntity, TPrimaryKey>(TEntity entity) where TEntity : class, IEntity<TPrimaryKey>;

        protected virtual void CheckAndSetId(object entityAsObj)
        {
            var entity = entityAsObj as IEntity<Guid>;
            if (entity != null && entity.Id == Guid.Empty)
            {
                Type entityType = entityAsObj.GetType();
                PropertyInfo idProperty = entityType.GetProperty("Id");
                var dbGeneratedAttr = ReflectionHelper.GetSingleAttributeOrDefault<DatabaseGeneratedAttribute>(idProperty);
                if (dbGeneratedAttr == null || dbGeneratedAttr.DatabaseGeneratedOption == DatabaseGeneratedOption.None)
                {
                    entity.Id = GuidGenerator.Create();
                }
            }
        }
    }
}
