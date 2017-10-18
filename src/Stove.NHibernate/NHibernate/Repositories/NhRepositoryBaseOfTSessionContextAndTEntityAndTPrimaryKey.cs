using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using NHibernate;
using NHibernate.Linq;

using Stove.Collections.Extensions;
using Stove.Domain.Entities;
using Stove.Domain.Repositories;
using Stove.NHibernate.Enrichments;

namespace Stove.NHibernate.Repositories
{
    public class NhRepositoryBase<TSessionContext, TEntity, TPrimaryKey> : StoveRepositoryBase<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
        where TSessionContext : StoveSessionContext
    {
        private readonly ISessionContextProvider<TSessionContext> _sessionProvider;

        public NhRepositoryBase(ISessionContextProvider<TSessionContext> sessionProvider)
        {
            _sessionProvider = sessionProvider;
        }

        public virtual ISession Session => _sessionProvider.GetSession();

        public override IQueryable<TEntity> GetAll()
        {
            return Session.Query<TEntity>();
        }

        public override IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            if (propertySelectors.IsNullOrEmpty()) return GetAll();

            IQueryable<TEntity> query = GetAll();

            foreach (Expression<Func<TEntity, object>> propertySelector in propertySelectors)
            {
                //TODO: Test if NHibernate supports multiple fetch.
                query = query.Fetch(propertySelector);
            }

            return query;
        }

        public override TEntity FirstOrDefault(TPrimaryKey id)
        {
            return Session.Get<TEntity>(id);
        }

        public override TEntity Load(TPrimaryKey id)
        {
            return Session.Load<TEntity>(id);
        }

        public override TEntity Insert(TEntity entity)
        {
            Session.Save(entity);
            return entity;
        }

        public override TEntity InsertOrUpdate(TEntity entity)
        {
            Session.SaveOrUpdate(entity);
            return entity;
        }

        public override Task<TEntity> InsertOrUpdateAsync(TEntity entity)
        {
            return Task.FromResult(InsertOrUpdate(entity));
        }

        public override TEntity Update(TEntity entity)
        {
            Session.Update(entity);
            return entity;
        }

        public override void Delete(TEntity entity)
        {
            if (entity is ISoftDelete delete)
            {
                delete.IsDeleted = true;
                Update(entity);
            }
            else Session.Delete(entity);
        }

        public override void Delete(TPrimaryKey id)
        {
            Delete(Session.Load<TEntity>(id));
        }
    }
}
