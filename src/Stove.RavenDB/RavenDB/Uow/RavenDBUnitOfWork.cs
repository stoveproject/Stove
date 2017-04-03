using System.Threading.Tasks;

using Autofac.Extras.IocManager;

using Raven.Client;

using Stove.Domain.Uow;

namespace Stove.RavenDB.Uow
{
    public class RavenDBUnitOfWork : UnitOfWorkBase, ITransientDependency
    {
        private readonly IDocumentStore _documentStore;

        public RavenDBUnitOfWork(
            IConnectionStringResolver connectionStringResolver,
            IUnitOfWorkDefaultOptions defaultOptions,
            IUnitOfWorkFilterExecuter filterExecuter,
            IDocumentStore documentStore)
            : base(connectionStringResolver, defaultOptions, filterExecuter)
        {
            _documentStore = documentStore;
        }

        public IDocumentSession Session { get; private set; }

        protected override void BeginUow()
        {
            Session = _documentStore.OpenSession();
        }

        public override void SaveChanges()
        {
            Session.SaveChanges();
        }

        public override Task SaveChangesAsync()
        {
            SaveChanges();
            return Task.FromResult(0);
        }

        protected override void CompleteUow()
        {
            SaveChanges();
        }

        protected override Task CompleteUowAsync()
        {
            CompleteUow();
            return Task.FromResult(0);
        }

        protected override void DisposeUow()
        {
            Session.Dispose();
        }
    }
}
