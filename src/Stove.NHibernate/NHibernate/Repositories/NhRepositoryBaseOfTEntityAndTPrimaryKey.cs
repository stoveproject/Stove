using Stove.Domain.Entities;
using Stove.Domain.Repositories;
using Stove.NHibernate.Enrichments;

namespace Stove.NHibernate.Repositories
{
    /// <summary>
    ///     Base class for all repositories those uses NHibernate.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TSessionContext"></typeparam>
    public class NhRepositoryBase<TSessionContext, TEntity> : NhRepositoryBase<TSessionContext, TEntity, int>, IRepository<TEntity>
        where TSessionContext : StoveSessionContext
        where TEntity : class, IEntity<int>

    {
        public NhRepositoryBase(ISessionProvider sessionProvider) : base(sessionProvider)
        {
        }
    }
}
