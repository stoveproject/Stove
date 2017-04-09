using Stove.Domain.Entities;

namespace Stove.RavenDB.Filters.Action
{
    public class NullRavenActionFilterExecuter : IRavenActionFilterExecuter
    {
        public static readonly NullRavenActionFilterExecuter Instance = new NullRavenActionFilterExecuter();

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
