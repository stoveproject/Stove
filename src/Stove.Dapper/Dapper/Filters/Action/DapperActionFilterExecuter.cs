using Autofac.Extras.IocManager;

using Stove.Domain.Entities;
using Stove.Domain.Uow.DynamicFilters.Action;

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
            var filter = _scopeResolver.Resolve<CreationAuditActionFilter>();
            filter.ExecuteFilter<TEntity, TPrimaryKey>(entity);
        }

        public void ExecuteModificationAuditFilter<TEntity, TPrimaryKey>(TEntity entity) where TEntity : class, IEntity<TPrimaryKey>
        {
            var filter = _scopeResolver.Resolve<ModificationAuditActionFilter>();
            filter.ExecuteFilter<TEntity, TPrimaryKey>(entity);
        }

        public void ExecuteDeletionAuditFilter<TEntity, TPrimaryKey>(TEntity entity) where TEntity : class, IEntity<TPrimaryKey>
        {
            var filter = _scopeResolver.Resolve<DeletionAuditActionFilter>();
            filter.ExecuteFilter<TEntity, TPrimaryKey>(entity);
        }
    }
}
