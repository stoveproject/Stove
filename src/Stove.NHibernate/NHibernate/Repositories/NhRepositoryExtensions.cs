using System;

using NHibernate;

using Stove.Domain.Entities;
using Stove.Domain.Repositories;
using Stove.Reflection;

namespace Stove.NHibernate.Repositories
{
    public static class NhRepositoryExtensions
    {
        public static ISession GetSession<TEntity, TPrimaryKey>(this IRepository<TEntity, TPrimaryKey> repository)
            where TEntity : class, IEntity<TPrimaryKey>
        {
            if (ProxyHelper.UnProxyOrSelf(repository) is IRepositoryWithSession repositoryWithSession)
            {
                return repositoryWithSession.GetSession();
            }

            throw new ArgumentException("Given repository does not implement IRepositoryWithSession", nameof(repository));
        }
    }
}
