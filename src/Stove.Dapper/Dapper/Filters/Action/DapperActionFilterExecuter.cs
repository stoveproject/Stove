using Autofac.Extras.IocManager;

using Stove.Domain.Entities;

namespace Stove.Dapper.Filters.Action
{
    public class DapperActionFilterExecuter : IDapperActionFilterExecuter, ITransientDependency
    {
        private readonly IScopeResolver _scopeResolver;

        public DapperActionFilterExecuter(IScopeResolver scopeResolver)
        {
            _scopeResolver = scopeResolver;
        }

        public void ExecuteCreationAuditFilter<TEntity, TPrimaryKey>(TEntity entity) where TEntity : class, IEntity<TPrimaryKey>
        {
            IDapperActionFilter filter = _scopeResolver.Resolve<CreationAuditDapperActionFilter>();
            filter.ExecuteFilter<TEntity, TPrimaryKey>(entity);
        }

        public void ExecuteModificationAuditFilter<TEntity, TPrimaryKey>(TEntity entity) where TEntity : class, IEntity<TPrimaryKey>
        {
            IDapperActionFilter filter = _scopeResolver.Resolve<ModificationAuditDapperActionFilter>();
            filter.ExecuteFilter<TEntity, TPrimaryKey>(entity);
        }

        public void ExecuteDeletionAuditFilter<TEntity, TPrimaryKey>(TEntity entity) where TEntity : class, IEntity<TPrimaryKey>
        {
            IDapperActionFilter filter = _scopeResolver.Resolve<DeletionAuditDapperActionFilter>();
            filter.ExecuteFilter<TEntity, TPrimaryKey>(entity);
        }
    }
}
