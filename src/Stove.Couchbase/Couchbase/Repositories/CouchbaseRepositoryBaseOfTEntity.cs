using System.Linq;

using Couchbase;
using Couchbase.Linq;

using Stove.Couchbase.Filters.Action;
using Stove.Domain.Entities;
using Stove.Domain.Repositories;
using Stove.Events.Bus.Entities;

namespace Stove.Couchbase.Repositories
{
    public class CouchbaseRepositoryBase<TEntity> : StoveRepositoryBase<TEntity, string> where TEntity : class, IEntity<string>
    {
        private readonly ISessionProvider _sessionProvider;

        public CouchbaseRepositoryBase(
            ISessionProvider sessionProvider)
        {
            _sessionProvider = sessionProvider;

            GuidGenerator = SequentialGuidGenerator.Instance;
            EntityChangeEventHelper = NullEntityChangeEventHelper.Instance;
            ActionFilterExecuter = NullCouchbaseActionFilterExecuter.Instance;
        }

        public ICouchbaseActionFilterExecuter ActionFilterExecuter { get; set; }

        public IEntityChangeEventHelper EntityChangeEventHelper { get; set; }

        public IGuidGenerator GuidGenerator { get; set; }

        public IBucketContext Session => _sessionProvider.Session;

        public override IQueryable<TEntity> GetAll()
        {
            return Session.Query<TEntity>();
        }

        public override TEntity Insert(TEntity entity)
        {
            entity.Id = GuidGenerator.Create().ToString("N");
            ActionFilterExecuter.ExecuteCreationAuditFilter<TEntity, string>(entity);

            EntityChangeEventHelper.TriggerEntityCreatingEvent(entity);

            IDocumentResult<TEntity> result = Session.Bucket.Insert(new Document<TEntity>
            {
                Content = entity,
                Id = $"{typeof(TEntity).Name}:{entity.Id}"
            });

            result.EnsureSuccess();

            EntityChangeEventHelper.TriggerEntityCreatedEventOnUowCompleted(entity);

            return result.Content;
        }

        public override TEntity Update(TEntity entity)
        {
            ActionFilterExecuter.ExecuteModificationAuditFilter<TEntity, string>(entity);

            EntityChangeEventHelper.TriggerEntityUpdatingEvent(entity);

            IDocumentResult<TEntity> result = Session.Bucket.Upsert(new Document<TEntity>
            {
                Content = entity,
                Id = $"{typeof(TEntity).Name}:{entity.Id}"
            });

            result.EnsureSuccess();

            EntityChangeEventHelper.TriggerEntityUpdatedEventOnUowCompleted(entity);

            return result.Content;
        }

        public override void Delete(TEntity entity)
        {
            ActionFilterExecuter.ExecuteDeletionAuditFilter<TEntity, string>(entity);

            EntityChangeEventHelper.TriggerEntityDeletingEvent(entity);

            if (entity is ISoftDelete)
            {
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

            EntityChangeEventHelper.TriggerEntityDeletedEventOnUowCompleted(entity);
        }

        public override void Delete(string id)
        {
            Session.Bucket.Remove(id).EnsureSuccess();
        }
    }
}
