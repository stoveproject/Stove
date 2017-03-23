using System.Data;

using Autofac.Extras.IocManager;

using NHibernate.Transaction;

using Stove.Extensions;
using Stove.Orm;
using Stove.Reflection;

namespace Stove.NHibernate
{
    public class NhActiveTransactionOrConnectionProvider : IActiveTransactionOrConnectionProvider, ISingletonDependency
    {
        private readonly ISessionProvider _sessionProvider;

        public NhActiveTransactionOrConnectionProvider(ISessionProvider sessionProvider)
        {
            _sessionProvider = sessionProvider;
        }

        public IDbTransaction GetActiveTransaction(ActiveTransactionOrConnectionProviderArgs args)
        {
            var adoTransaction = _sessionProvider.Session.Transaction.As<AdoTransaction>();
            var dbTransaction = TypeHelper.GetInstanceField(typeof(AdoTransaction), adoTransaction, "trans").As<IDbTransaction>();
            return dbTransaction;
        }

        public IDbConnection GetActiveConnection(ActiveTransactionOrConnectionProviderArgs args)
        {
            return _sessionProvider.Session.Connection;
        }
    }
}
