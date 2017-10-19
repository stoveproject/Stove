using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Autofac.Extras.IocManager;

using NHibernate;

using Stove.Collections.Extensions;
using Stove.Domain.Uow;
using Stove.NHibernate.Enrichments;
using Stove.Transactions.Extensions;

namespace Stove.NHibernate.Uow
{
    /// <inheritdoc cref="UnitOfWorkBase" />
    /// <summary>
    ///     Implements Unit of work for NHibernate.
    /// </summary>
    public class NhUnitOfWork : UnitOfWorkBase, ITransientDependency
    {
        private readonly ISessionFactoryProvider _sessionFactoryProvider;
       
        /// <summary>
        ///     Creates a new instance of <see cref="NhUnitOfWork" />.
        /// </summary>
        public NhUnitOfWork(
            IConnectionStringResolver connectionStringResolver,
            IUnitOfWorkDefaultOptions defaultOptions,
            IUnitOfWorkFilterExecuter filterExecuter,
            ISessionFactoryProvider sessionFactoryProvider)
            : base(
                connectionStringResolver,
                defaultOptions,
                filterExecuter)
        {
            _sessionFactoryProvider = sessionFactoryProvider;

            ActiveSessions = new Dictionary<string, ISession>();
            ActiveTransactions = new Dictionary<string, ActiveTransactionInfo>();
        }

        protected Dictionary<string, ISession> ActiveSessions { get; set; }

        protected Dictionary<string, ActiveTransactionInfo> ActiveTransactions { get; set; }

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
            
        }

        public override void SaveChanges()
        {
            foreach (ISession session in ActiveSessions.Values)
            {
                session.Flush();
            }
        }

        public override async Task SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (ISession session in GetAllActiveSessions())
            {
                await session.FlushAsync(cancellationToken);
            }
        }

        /// <summary>
        ///     Commits transaction and closes database connection.
        /// </summary>
        protected override void CompleteUow()
        {
            SaveChanges();

            foreach (ActiveTransactionInfo activeTransactionInfo in ActiveTransactions.Values)
            {
                activeTransactionInfo.Transaction.Commit();
            }
        }

        protected override Task CompleteUowAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return SaveChangesAsync(cancellationToken).ContinueWith(async task =>
            {
                foreach (ITransaction activeTransaction in GetAllActiveTransactions())
                {
                    await activeTransaction.CommitAsync(cancellationToken);
                }
            }, cancellationToken);
        }

        /// <summary>
        ///     Rollbacks transaction and closes database connection.
        /// </summary>
        protected override void DisposeUow()
        {
            foreach (ActiveTransactionInfo activeTransaction in ActiveTransactions.Values)
            {
                foreach (ISession session in activeTransaction.AttendedSessions)
                {
                    session.Dispose();
                }

                activeTransaction.Transaction.Dispose();
                activeTransaction.StarterSession.Dispose();
            }

            foreach (ISession activeSession in ActiveSessions.Values)
            {
                activeSession.Dispose();
            }

            ActiveSessions.Clear();
            ActiveTransactions.Clear();
        }

        public ISession GetOrCreateSession<TSessionContext>() where TSessionContext : StoveSessionContext
        {
            ISessionFactory sessionFactory = _sessionFactoryProvider.GetSessionFactory<TSessionContext>();
            var connectionStringResolveArgs = new ConnectionStringResolveArgs();
            connectionStringResolveArgs["SessionContextType"] = typeof(TSessionContext);
            string connectionString = ResolveConnectionString(connectionStringResolveArgs);

            string sessionKey = typeof(TSessionContext).FullName + "#" + connectionString;

            if (!ActiveSessions.TryGetValue(sessionKey, out ISession session))
            {
                if (Options.IsTransactional == true)
                {
                    ActiveTransactionInfo activeTransactionInfo = ActiveTransactions.GetOrDefault(connectionString);
                    if (activeTransactionInfo == null)
                    {
                        session = DbConnection != null
                            ? sessionFactory.WithOptions().Connection(DbConnection).OpenSession()
                            : sessionFactory.OpenSession();

                        ITransaction transaction = Options.IsolationLevel.HasValue
                            ? session.BeginTransaction(Options.IsolationLevel.Value.ToSystemDataIsolationLevel())
                            : session.BeginTransaction();

                        activeTransactionInfo = new ActiveTransactionInfo(transaction, session.Connection, session);
                        ActiveTransactions[connectionString] = activeTransactionInfo;
                    }
                    else
                    {
                        session = activeTransactionInfo.StarterSession;

                        activeTransactionInfo.AttendedSessions.Add(session);
                    }
                }
                else
                {
                    session = DbConnection != null
                        ? sessionFactory.OpenSessionWithConnection(DbConnection)
                        : sessionFactory.OpenSession();
                }

                if (Options.Timeout.HasValue && session.Connection.ConnectionTimeout != 0)
                {
                    //TODO
                }

                ActiveSessions[sessionKey] = session;
            }

            return session;
        }

        protected virtual IReadOnlyList<ISession> GetAllActiveSessions()
        {
            return ActiveSessions.Select(x => x.Value).ToList();
        }

        protected virtual IReadOnlyList<ITransaction> GetAllActiveTransactions()
        {
            return ActiveTransactions.Select(x => x.Value.Transaction).ToList();
        }
    }

    public class ActiveTransactionInfo
    {
        public ActiveTransactionInfo(ITransaction transaction, DbConnection connection, ISession starterSession)
        {
            Transaction = transaction;
            Connection = connection;
            StarterSession = starterSession;
            AttendedSessions = new List<ISession>();
        }

        public ITransaction Transaction { get; }

        public DbConnection Connection { get; }

        public ISession StarterSession { get; }

        public List<ISession> AttendedSessions { get; }
    }
}
