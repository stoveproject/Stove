using System.Collections.Generic;
using System.Threading.Tasks;

using Stove.Domain.Entities;

namespace Stove.Dapper.Dapper.Repositories
{
    public abstract class StoveDapperRepositoryBase<TEntity, TPrimaryKey> : IDapperRepository<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
    {
        public abstract TEntity Get(TPrimaryKey id);

        public abstract IEnumerable<TEntity> GetList();

        public abstract IEnumerable<TEntity> GetList(object predicate);

        public virtual Task<TEntity> GetAsync(TPrimaryKey id)
        {
            return Task.FromResult(Get(id));
        }

        public virtual Task<IEnumerable<TEntity>> GetListAsync()
        {
            return Task.FromResult(GetList());
        }

        public virtual Task<IEnumerable<TEntity>> GetListAsync(object predicate)
        {
            return Task.FromResult(GetList(predicate));
        }

        public abstract int Count(object predicate);

        public virtual Task<int> CountAsync(object predicate)
        {
            return Task.FromResult(Count(predicate));
        }

        public abstract IEnumerable<TEntity> Query(string query, object parameters);

        public virtual Task<IEnumerable<TEntity>> QueryAsync(string query, object parameters)
        {
            return Task.FromResult(Query(query, parameters));
        }

        public abstract IEnumerable<TEntity> GetSet(object predicate, int firstResult, int maxResults, string sortingProperty, bool ascending = true);

        public virtual Task<IEnumerable<TEntity>> GetSetAsync(object predicate, int firstResult, int maxResults, string sortingProperty, bool ascending = true)
        {
            return Task.FromResult(GetSet(predicate, firstResult, maxResults, sortingProperty, ascending));
        }

        public Task<IEnumerable<TEntity>> GetListPagedAsync(int pageNumber, int itemsPerPage, string conditions, string order, object predicate, string sortingProperty, bool ascending = true)
        {
            return Task.FromResult(GetListPaged(pageNumber, itemsPerPage, conditions, order, predicate, sortingProperty, ascending));
        }

        public abstract IEnumerable<TEntity> GetListPaged(int pageNumber, int itemsPerPage, string conditions, string order, object predicate, string sortingProperty, bool ascending = true);
    }
}
