using System;
using System.Data;
using System.Data.Entity;
using System.Reflection;

using Autofac.Extras.IocManager;

using Stove.Data;
using Stove.Extensions;

namespace Stove.EntityFramework
{
    public class EfActiveTransactionProvider : IActiveTransactionProvider, ITransientDependency
    {
        private static readonly MethodInfo getDbContextMethod = typeof(IDbContextProvider<StoveDbContext>)
            .GetMethod(nameof(IDbContextProvider<StoveDbContext>.GetDbContext));

        private readonly IScopeResolver _scopeResolver;

        public EfActiveTransactionProvider(IScopeResolver scopeResolver)
        {
            _scopeResolver = scopeResolver;
        }

        public IDbTransaction GetActiveTransaction(ActiveTransactionProviderArgs args)
        {
            return GetDbContext(args).Database.CurrentTransaction?.UnderlyingTransaction;
        }

        public IDbConnection GetActiveConnection(ActiveTransactionProviderArgs args)
        {
            return GetDbContext(args).Database.Connection;
        }

        private DbContext GetDbContext(ActiveTransactionProviderArgs args)
        {
            var dbContextType = (Type)args["ContextType"];
            Type dbContextProviderType = typeof(IDbContextProvider<>).MakeGenericType(dbContextType);
            object dbContextProvider = _scopeResolver.Resolve(dbContextProviderType);
            var dbContext = getDbContextMethod.Invoke(dbContextProvider, null).As<DbContext>();
            return dbContext;
        }
    }
}
