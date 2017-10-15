using Stove.Domain.Entities;

namespace Stove.Couchbase.Filters.Action
{
    public class NullCouchbaseActionFilterExecuter : ICouchbaseActionFilterExecuter
    {
        public static NullCouchbaseActionFilterExecuter Instance = new NullCouchbaseActionFilterExecuter();

        public void ExecuteCreationAuditFilter<TEntity, TPrimaryKey>(TEntity entity) where TEntity : class, IEntity<TPrimaryKey>
        {
        }

        public void ExecuteModificationAuditFilter<TEntity, TPrimaryKey>(TEntity entity) where TEntity : class, IEntity<TPrimaryKey>
        {
        }

        public void ExecuteDeletionAuditFilter<TEntity, TPrimaryKey>(TEntity entity) where TEntity : class, IEntity<TPrimaryKey>
        {
        }
    }
}
