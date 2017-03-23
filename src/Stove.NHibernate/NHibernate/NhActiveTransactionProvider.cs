using System;
using System.Data;

using NHibernate.Transaction;

using Stove.Extensions;
using Stove.Orm;
using Stove.Reflection;

namespace Stove.NHibernate
{
    public class NhActiveTransactionProvider : IActiveTransactionProvider
    {
        private readonly ISessionProvider _sessionProvider;

        public NhActiveTransactionProvider(ISessionProvider sessionProvider)
        {
            _sessionProvider = sessionProvider;
        }

        public IDbTransaction GetActiveTransaction(Type dbContextType = null)
        {
            var adoTransaction = _sessionProvider.Session.Transaction.As<AdoTransaction>();
            var dbTransaction = TypeHelper.GetInstanceField(typeof(IDbTransaction), adoTransaction, "trans").As<IDbTransaction>();
            return dbTransaction;
        }

        public IDbConnection GetActiveConnection(Type dbContextType = null)
        {
            return _sessionProvider.Session.Connection;
        }
    }
}
