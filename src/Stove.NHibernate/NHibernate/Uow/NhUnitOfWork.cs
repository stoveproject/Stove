using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

using Autofac.Extras.IocManager;

using NHibernate;

using Stove.Domain.Uow;
using Stove.Transactions.Extensions;

namespace Stove.NHibernate.Uow
{
    /// <summary>
    ///     Implements Unit of work for NHibernate.
    /// </summary>
    public class NhUnitOfWork : UnitOfWorkBase, ITransientDependency
    {
        private readonly ISessionFactory _sessionFactory;
        private ITransaction _transaction;

        /// <summary>
        ///     Creates a new instance of <see cref="NhUnitOfWork" />.
        /// </summary>
        public NhUnitOfWork(
            ISessionFactory sessionFactory,
            IConnectionStringResolver connectionStringResolver,
            IUnitOfWorkDefaultOptions defaultOptions,
            IUnitOfWorkFilterExecuter filterExecuter)
            : base(
                connectionStringResolver,
                defaultOptions,
                filterExecuter)
        {
            _sessionFactory = sessionFactory;
        }

        /// <summary>
        ///     Gets NHibernate session object to perform queries.
        /// </summary>
        public ISession Session { get; private set; }

        /// <summary>
        ///     <see cref="NhUnitOfWork" /> uses this DbConnection if it's set.
        ///     This is usually set in tests.
        /// </summary>
        public DbConnection DbConnection { get; set; }

        protected override void BeginUow()
        {
            Session = DbConnection != null
                ? _sessionFactory.OpenSessionWithConnection(DbConnection)
                : _sessionFactory.OpenSession();

            if (Options.IsTransactional == true)
            {
                _transaction = Options.IsolationLevel.HasValue
                    ? Session.BeginTransaction(Options.IsolationLevel.Value.ToSystemDataIsolationLevel())
                    : Session.BeginTransaction();
            }
        }

        public override void SaveChanges()
        {
            Session.Flush();
        }

        public override Task SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Session.FlushAsync(cancellationToken);
        }

        /// <summary>
        ///     Commits transaction and closes database connection.
        /// </summary>
        protected override void CompleteUow()
        {
            SaveChanges();
            _transaction?.Commit();
        }

        protected override Task CompleteUowAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return SaveChangesAsync(cancellationToken).ContinueWith(task => _transaction?.CommitAsync(cancellationToken), cancellationToken);
        }

        /// <summary>
        ///     Rollbacks transaction and closes database connection.
        /// </summary>
        protected override void DisposeUow()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }

            Session.Dispose();
        }
    }
}
