using System;
using System.Data;
using System.Reflection;

using Autofac.Extras.IocManager;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

using Stove.Data;
using Stove.Extensions;

namespace Stove.EntityFrameworkCore
{
    public class EfCoreActiveTransactionProvider : IActiveTransactionProvider, ITransientDependency
    {
        private static readonly MethodInfo getDbContextMethod = typeof(IDbContextProvider<StoveDbContext>)
            .GetMethod(nameof(IDbContextProvider<StoveDbContext>.GetDbContext));

        private readonly IScopeResolver _scopeResolver;

        public EfCoreActiveTransactionProvider(IScopeResolver scopeResolver)
        {
            _scopeResolver = scopeResolver;
        }

        public IDbTransaction GetActiveTransaction(ActiveTransactionProviderArgs args)
        {
            return GetDbContext(args).Database.CurrentTransaction?.GetDbTransaction();
        }

        public IDbConnection GetActiveConnection(ActiveTransactionProviderArgs args)
        {
            return GetDbContext(args).Database.GetDbConnection();
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
