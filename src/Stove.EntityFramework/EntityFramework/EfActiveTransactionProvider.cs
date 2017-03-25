using System;
using System.Data;
using System.Data.Entity;
using System.Reflection;

using Autofac.Extras.IocManager;

using Stove.Extensions;
using Stove.Orm;
using Stove.Transactions;

namespace Stove.EntityFramework
{
    public class EfActiveTransactionProvider : IActiveTransactionProvider, ITransientDependency
    {
        private readonly IScopeResolver _scopeResolver;

        public EfActiveTransactionProvider(IScopeResolver scopeResolver)
        {
            _scopeResolver = scopeResolver;
        }

        public IDbTransaction GetActiveTransaction(ActiveTransactionProviderArgs args)
        {
            var dbContextType = (Type)args["ContextType"];
            Type dbContextProviderType = typeof(IDbContextProvider<>).MakeGenericType(dbContextType);
            object dbContextProvider = _scopeResolver.Resolve(dbContextProviderType);
            MethodInfo method = dbContextProvider.GetType().GetMethod("GetDbContext");
            var dbContext = method.Invoke(dbContextProvider, null).As<DbContext>();

            return dbContext.Database.CurrentTransaction.UnderlyingTransaction;
        }

        public IDbConnection GetActiveConnection(ActiveTransactionProviderArgs args)
        {
            var dbContextType = (Type)args["ContextType"];
            Type dbContextProviderType = typeof(IDbContextProvider<>).MakeGenericType(dbContextType);
            object dbContextProvider = _scopeResolver.Resolve(dbContextProviderType);
            MethodInfo method = dbContextProvider.GetType().GetMethod("GetDbContext");
            var dbContext = method.Invoke(dbContextProvider, null).As<DbContext>();

            return dbContext.Database.Connection;
        }
    }
}
