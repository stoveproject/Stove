using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Threading.Tasks;

using Dapper;

using DapperExtensions;

using Stove.Domain.Entities;
using Stove.Domain.Repositories;
using Stove.EntityFramework.EntityFramework;

namespace Stove.Dapper.Dapper.Repositories
{
    public class DapperRepositoryBase<TDbContext, TEntity, TPrimaryKey> : StoveDapperRepositoryBase<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
        where TDbContext : DbContext
    {
        private readonly IDbContextProvider<TDbContext> _dbContextProvider;

        public DapperRepositoryBase(IDbContextProvider<TDbContext> dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }

        public virtual TDbContext Context
        {
            get { return _dbContextProvider.GetDbContext(); }
        }

        public virtual DbConnection Connection
        {
            get { return Context.Database.Connection; }
        }

        public virtual IDbTransaction ActiveTransaction
        {
            get { return Context.Database.CurrentTransaction.UnderlyingTransaction; }
        }

        public override TEntity Get(TPrimaryKey id)
        {
            return Connection.Get<TEntity>(id, ActiveTransaction);
        }

        public override IEnumerable<TEntity> GetList()
        {
            return Connection.GetList<TEntity>(transaction: ActiveTransaction);
        }

        public override IEnumerable<TEntity> GetList(object predicate)
        {
            return Connection.GetList<TEntity>(predicate, transaction: ActiveTransaction);
        }

        public override IEnumerable<TEntity> GetListPaged(
            int pageNumber,
            int itemsPerPage,
            string conditions,
            string order,
            object predicate,
            string sortingProperty,
            bool ascending = true)
        {
            return Connection.GetPage<TEntity>(
                predicate: predicate,
                sort: new List<ISort>() { new Sort() { Ascending = ascending, PropertyName = sortingProperty } },
                page: pageNumber,
                resultsPerPage: itemsPerPage,
                transaction: ActiveTransaction);
        }

        public override Task<TEntity> GetAsync(TPrimaryKey id)
        {
            return Connection.GetAsync<TEntity>(id, ActiveTransaction);
        }

        public override Task<IEnumerable<TEntity>> GetListAsync()
        {
            return Connection.GetListAsync<TEntity>(transaction: ActiveTransaction);
        }

        public override Task<IEnumerable<TEntity>> GetListAsync(object predicate)
        {
            return Connection.GetListAsync<TEntity>(predicate, transaction: ActiveTransaction);
        }

        public override int Count(object predicate)
        {
            return Connection.Count<TEntity>(predicate, ActiveTransaction);
        }

        public override Task<int> CountAsync(object predicate)
        {
            return Connection.CountAsync<TEntity>(predicate, ActiveTransaction);
        }

        public override IEnumerable<TEntity> Query(string query, object parameters)
        {
            return Connection.Query<TEntity>(query, parameters, ActiveTransaction);
        }

        public override Task<IEnumerable<TEntity>> QueryAsync(string query, object parameters)
        {
            return Connection.QueryAsync<TEntity>(query, parameters, ActiveTransaction);
        }

        public override IEnumerable<TAny> Query<TAny>(string query)
        {
            return Connection.Query<TAny>(query, transaction: ActiveTransaction);
        }

        public override Task<IEnumerable<TAny>> QueryAsync<TAny>(string query)
        {
            return Connection.QueryAsync<TAny>(query, transaction: ActiveTransaction);
        }

        public override IEnumerable<TEntity> GetSet(object predicate, int firstResult, int maxResults, string sortingProperty, bool ascending = true)
        {
            return Connection.GetSet<TEntity>(
            predicate: predicate,
            sort: new List<ISort>() { new Sort() { Ascending = ascending, PropertyName = sortingProperty } },
            firstResult: firstResult,
            maxResults: maxResults,
            transaction: ActiveTransaction);
        }

        public override Task<IEnumerable<TEntity>> GetSetAsync(object predicate, int firstResult, int maxResults, string sortingProperty, bool ascending = true)
        {
            return Connection.GetSetAsync<TEntity>(
             predicate: predicate,
             sort: new List<ISort>() { new Sort() { Ascending = ascending, PropertyName = sortingProperty } },
             firstResult: firstResult,
             maxResults: maxResults,
             transaction: ActiveTransaction);
        }

        public override IEnumerable<TAny> Query<TAny>(string query, object parameters)
        {
            return Connection.Query<TAny>(query, param: parameters, transaction: ActiveTransaction); ;
        }

        public override Task<IEnumerable<TAny>> QueryAsync<TAny>(string query, object parameters)
        {
            return Connection.QueryAsync<TAny>(query, param: parameters, transaction: ActiveTransaction);
        }
    }
}
