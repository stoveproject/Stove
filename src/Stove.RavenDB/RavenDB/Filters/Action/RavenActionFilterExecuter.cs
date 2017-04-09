using Autofac.Extras.IocManager;

using Stove.Domain.Entities;

namespace Stove.RavenDB.Filters.Action
{
    public class RavenActionFilterExecuter : IRavenActionFilterExecuter, ITransientDependency
    {
        private readonly IScopeResolver _scopeResolver;

        public RavenActionFilterExecuter(IScopeResolver scopeResolver)
        {
            _scopeResolver = scopeResolver;
        }

        public void ExecuteCreationAuditFilter<TEntity, TPrimaryKey>(TEntity entity) where TEntity : class, IEntity<TPrimaryKey>
        {
            _scopeResolver.Resolve<CreationAuditRavenActionFilter>().ExecuteFilter<TEntity, TPrimaryKey>(entity);
        }

        public void ExecuteModificationAuditFilter<TEntity, TPrimaryKey>(TEntity entity) where TEntity : class, IEntity<TPrimaryKey>
        {
            _scopeResolver.Resolve<ModificationAuditRavenActionFilter>().ExecuteFilter<TEntity, TPrimaryKey>(entity);
        }

        public void ExecuteDeletionAuditFilter<TEntity, TPrimaryKey>(TEntity entity) where TEntity : class, IEntity<TPrimaryKey>
        {
            _scopeResolver.Resolve<DeletionAuditRavenActionFilter>().ExecuteFilter<TEntity, TPrimaryKey>(entity);
        }
    }
}
