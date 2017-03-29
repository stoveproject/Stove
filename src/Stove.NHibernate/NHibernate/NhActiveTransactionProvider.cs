using System.Data;

using Autofac.Extras.IocManager;

using NHibernate.Transaction;

using Stove.Extensions;
using Stove.Orm;
using Stove.Reflection;
using Stove.Transactions;

namespace Stove.NHibernate
{
    public class NhActiveTransactionProvider : IActiveTransactionProvider, ITransientDependency
    {
        private readonly ISessionProvider _sessionProvider;

        public NhActiveTransactionProvider(ISessionProvider sessionProvider)
        {
            _sessionProvider = sessionProvider;
        }

        public IDbTransaction GetActiveTransaction(ActiveTransactionProviderArgs args)
        {
            var adoTransaction = _sessionProvider.Session.Transaction.As<AdoTransaction>();
            var dbTransaction = TypeHelper.GetInstanceField(typeof(AdoTransaction), adoTransaction, "trans").As<IDbTransaction>();
            return dbTransaction;
        }

        public IDbConnection GetActiveConnection(ActiveTransactionProviderArgs args)
        {
            return _sessionProvider.Session.Connection;
        }
    }
}
