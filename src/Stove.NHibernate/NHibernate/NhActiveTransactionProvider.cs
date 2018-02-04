using System;
using System.Data;
using System.Reflection;

using Autofac.Extras.IocManager;

using NHibernate;
using NHibernate.Transaction;

using Stove.Data;
using Stove.Extensions;
using Stove.NHibernate.Enrichments;

namespace Stove.NHibernate
{
    public class NhActiveTransactionProvider : IActiveTransactionProvider, ITransientDependency
    {
        private static readonly MethodInfo getSessionMethod = typeof(ISessionProvider).GetMethod(nameof(ISessionProvider.GetSession));
        private readonly IScopeResolver _scope;

        public NhActiveTransactionProvider(IScopeResolver scope)
        {
            _scope = scope;
        }

        public IDbTransaction GetActiveTransaction(ActiveTransactionProviderArgs args)
        {
            var adoTransaction = GetSession(args).Transaction.As<AdoTransaction>();
            var dbTransaction = GetInstanceField(typeof(AdoTransaction), adoTransaction, "trans").As<IDbTransaction>();
            return dbTransaction;
        }

        public IDbConnection GetActiveConnection(ActiveTransactionProviderArgs args)
        {
            return GetSession(args).Connection;
        }

        private object GetInstanceField(Type type, object instance, string fieldName)
        {
            return type.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                       ?.GetValue(instance);
        }

        private ISession GetSession(ActiveTransactionProviderArgs args)
        {
            var sessionContextType = (Type)args["SessionContextType"];
            var sessionContextProvider = _scope.Resolve<ISessionProvider>();
            MethodInfo method = getSessionMethod.MakeGenericMethod(sessionContextType);
            var session = method.Invoke(sessionContextProvider, null).As<ISession>();
            return session;
        }
    }
}
