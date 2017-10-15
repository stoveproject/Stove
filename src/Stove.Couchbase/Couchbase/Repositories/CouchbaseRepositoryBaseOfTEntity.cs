using System.Linq;

using Couchbase;
using Couchbase.Linq;

using Stove.Couchbase.Couchbase.Filters.Action;
using Stove.Domain.Entities;
using Stove.Domain.Repositories;
using Stove.Extensions;

namespace Stove.Couchbase.Couchbase.Repositories
{
    public class CouchbaseRepositoryBase<TEntity> : StoveRepositoryBase<TEntity, string> where TEntity : class, IEntity<string>
    {
        private readonly ICouchbaseActionFilterExecuter _actionFilterExecuter;
        private readonly ISessionProvider _sessionProvider;

        public CouchbaseRepositoryBase(
            ISessionProvider sessionProvider,
            ICouchbaseActionFilterExecuter actionFilterExecuter)
        {
            _sessionProvider = sessionProvider;
            _actionFilterExecuter = actionFilterExecuter;

            GuidGenerator = SequentialGuidGenerator.Instance;
        }

        public IGuidGenerator GuidGenerator { get; set; }

        public IBucketContext Session => _sessionProvider.Session;

        public override IQueryable<TEntity> GetAll()
        {
            return Session.Query<TEntity>();
        }

        public override TEntity Insert(TEntity entity)
        {
            entity.Id = GuidGenerator.Create().ToString("N");
            _actionFilterExecuter.ExecuteCreationAuditFilter<TEntity, string>(entity);
            IDocumentResult<TEntity> result = Session.Bucket.Insert(new Document<TEntity>
            {
                Content = entity,
                Id = $"{typeof(TEntity).Name}:{entity.Id}"
            });

            result.EnsureSuccess();

            return result.Content;
        }

        public override TEntity Update(TEntity entity)
        {
            _actionFilterExecuter.ExecuteModificationAuditFilter<TEntity, string>(entity);
            IDocumentResult<TEntity> result = Session.Bucket.Upsert(new Document<TEntity>
            {
                Content = entity,
                Id = $"{typeof(TEntity).Name}:{entity.Id}"
            });

            result.EnsureSuccess();

            return result.Content;
        }

        public override void Delete(TEntity entity)
        {
            _actionFilterExecuter.ExecuteDeletionAuditFilter<TEntity, string>(entity);

            if (entity is ISoftDelete)
            {
                entity.As<ISoftDelete>().IsDeleted = true;

                Session.Bucket.Upsert(new Document<TEntity>
                {
                    Content = entity,
                    Id = $"{typeof(TEntity).Name}:{entity.Id}"
                }).EnsureSuccess();
            }
            else
            {
                Session.Bucket.Remove(new Document<TEntity>
                {
                    Content = entity,
                    Id = $"{typeof(TEntity).Name}:{entity.Id}"
                }).EnsureSuccess();
            }
        }

        public override void Delete(string id)
        {
            Session.Bucket.Remove(id).EnsureSuccess();
        }
    }
}
