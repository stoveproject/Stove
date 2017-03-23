using System;
using System.Data;
using System.Data.Entity;
using System.Reflection;

using Autofac.Extras.IocManager;

using Stove.Extensions;
using Stove.Orm;

namespace Stove.EntityFramework
{
    public class EfActiveTransactionProvider : IActiveTransactionProvider, ITransientDependency
    {
        private readonly IScopeResolver _scopeResolver;

        public EfActiveTransactionProvider(IScopeResolver scopeResolver)
        {
            _scopeResolver = scopeResolver;
        }

        public IDbTransaction GetActiveTransaction(Type dbContextType = null)
        {
            Type dbContextProviderType = typeof(IDbContextProvider<>).MakeGenericType(dbContextType);
            object dbContextProvider = _scopeResolver.Resolve(dbContextProviderType);
            MethodInfo method = dbContextProvider.GetType().GetMethod("GetDbContext");
            var dbContext = method.Invoke(dbContextProvider, null).As<DbContext>();

            return dbContext.Database.CurrentTransaction.UnderlyingTransaction;
        }

        public IDbConnection GetActiveConnection(Type dbContextType = null)
        {
            Type dbContextProviderType = typeof(IDbContextProvider<>).MakeGenericType(dbContextType);
            object dbContextProvider = _scopeResolver.Resolve(dbContextProviderType);
            MethodInfo method = dbContextProvider.GetType().GetMethod("GetDbContext");
            var dbContext = method.Invoke(dbContextProvider, null).As<DbContext>();

            return dbContext.Database.Connection;
        }
    }
}
