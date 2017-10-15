using Autofac.Extras.IocManager;

using Stove.Domain.Entities;
using Stove.Domain.Uow.DynamicFilters.Action;

namespace Stove.Couchbase.Couchbase.Filters.Action
{
    public class CouchbaseActionFilterExecuter : ICouchbaseActionFilterExecuter, ITransientDependency
    {
        private readonly IScopeResolver _scopeResolver;

        public CouchbaseActionFilterExecuter(IScopeResolver scopeResolver)
        {
            _scopeResolver = scopeResolver;
        }

        public void ExecuteCreationAuditFilter<TEntity, TPrimaryKey>(TEntity entity) where TEntity : class, IEntity<TPrimaryKey>
        {
            _scopeResolver.Resolve<CreationAuditActionFilter>().ExecuteFilter<TEntity, TPrimaryKey>(entity);
        }

        public void ExecuteModificationAuditFilter<TEntity, TPrimaryKey>(TEntity entity) where TEntity : class, IEntity<TPrimaryKey>
        {
            _scopeResolver.Resolve<ModificationAuditActionFilter>().ExecuteFilter<TEntity, TPrimaryKey>(entity);
        }

        public void ExecuteDeletionAuditFilter<TEntity, TPrimaryKey>(TEntity entity) where TEntity : class, IEntity<TPrimaryKey>
        {
            _scopeResolver.Resolve<DeletionAuditActionFilter>().ExecuteFilter<TEntity, TPrimaryKey>(entity);
        }
    }
}
