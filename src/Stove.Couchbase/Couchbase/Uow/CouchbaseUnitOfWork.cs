using System.Threading;
using System.Threading.Tasks;

using Autofac.Extras.IocManager;

using Couchbase.Core;
using Couchbase.Linq;

using Stove.Domain.Uow;

namespace Stove.Couchbase.Uow
{
    public class CouchbaseUnitOfWork : UnitOfWorkBase, ITransientDependency
    {
        private readonly ICluster _cluster;

        public CouchbaseUnitOfWork(
            IConnectionStringResolver connectionStringResolver,
            IUnitOfWorkDefaultOptions defaultOptions,
            IUnitOfWorkFilterExecuter filterExecuter,
            ICluster cluster) : base(connectionStringResolver, defaultOptions, filterExecuter)
        {
            _cluster = cluster;
        }

        public IBucketContext Session { get; private set; }

        public IBucket Bucket { get; private set; }

        protected override void BeginUow()
        {
            Bucket = _cluster.OpenBucket();
            Session = new BucketContext(Bucket);
            Session.BeginChangeTracking();
        }

        public override void SaveChanges()
        {
            Session.SubmitChanges();
        }

        public override Task SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            SaveChanges();
            return Task.CompletedTask;
        }

        protected override void CompleteUow()
        {
            Session.EndChangeTracking();
            SaveChanges();
        }

        protected override Task CompleteUowAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            CompleteUow();
            return Task.CompletedTask;
        }

        protected override void DisposeUow()
        {
            Bucket.Dispose();
            _cluster.CloseBucket(Bucket);
        }
    }
}
