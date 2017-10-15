using System.Linq;

using Couchbase;
using Couchbase.Linq;

using Stove.Domain.Entities;
using Stove.Domain.Repositories;

namespace Stove.Couchbase.Couchbase.Repositories
{
    public class CouchbaseRepositoryBase<TEntity> : StoveRepositoryBase<TEntity, string> where TEntity : class, IEntity<string>
    {
        private readonly ISessionProvider _sessionProvider;

        public CouchbaseRepositoryBase(ISessionProvider sessionProvider)
        {
            _sessionProvider = sessionProvider;
        }

        public IBucketContext Session => _sessionProvider.Session;

        public override IQueryable<TEntity> GetAll()
        {
            return Session.Query<TEntity>();
        }

        public override TEntity Insert(TEntity entity)
        {
            IDocumentResult<TEntity> result = Session.Bucket.Insert(new Document<TEntity>
            {
                Content = entity,
                Id = entity.Id
            });

            result.EnsureSuccess();

            return result.Content;
        }

        public override TEntity Update(TEntity entity)
        {
            IDocumentResult<TEntity> result = Session.Bucket.Upsert(new Document<TEntity>
            {
                Content = entity,
                Id = entity.Id
            });

            result.EnsureSuccess();

            return result.Content;
        }

        public override void Delete(TEntity entity)
        {
            Session.Bucket.Remove(new Document<TEntity>
            {
                Content = entity,
                Id = entity.Id
            }).EnsureSuccess();
        }

        public override void Delete(string id)
        {
            Session.Bucket.Remove(id).EnsureSuccess();
        }
    }
}
