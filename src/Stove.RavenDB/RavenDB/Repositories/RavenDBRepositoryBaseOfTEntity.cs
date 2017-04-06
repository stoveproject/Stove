using Stove.Domain.Entities;
using Stove.Domain.Repositories;

namespace Stove.RavenDB.Repositories
{
    public class RavenDBRepositoryBase<TEntity> : RavenDBRepositoryBase<TEntity, int>, IRepository<TEntity> where TEntity : class, IEntity<int>
    {
        public RavenDBRepositoryBase(ISessionProvider sessionProvider) : base(sessionProvider)
        {
        }
    }
}
