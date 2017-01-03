using System;
using System.Collections.Generic;
using System.Reflection;

using Autofac.Extras.IocManager;

using FluentAssemblyScanner;

using Stove.Domain.Uow;
using Stove.EntityFramework.EntityFramework;
using Stove.EntityFramework.EntityFramework.Uow;

namespace Stove.EntityFramework
{
    public static class StoveEntityFrameworkRegistrationExtensions
    {
        public static IIocBuilder UseStoveEntityFramework(this IIocBuilder builder)
        {
            builder.RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly()));
            builder.RegisterServices(r => r.Register<IEfUnitOfWorkFilterExecuter, IEfUnitOfWorkFilterExecuter, EfDynamicFiltersUnitOfWorkFilterExecuter>());
            builder.RegisterServices(r => r.RegisterGeneric(typeof(IDbContextProvider<>), typeof(UnitOfWorkDbContextProvider<>)));
            builder.RegisterServices(r => r.Register<IUnitOfWorkDefaultOptions, UnitOfWorkDefaultOptions>(Lifetime.Singleton));

            List<Type> dbContextTypes = AssemblyScanner.FromAssemblyInDirectory(new AssemblyFilter(string.Empty))
                                                       .BasedOn<StoveDbContext>()
                                                       .Filter()
                                                       .Classes()
                                                       .NonStatic()
                                                       .Scan();

            dbContextTypes.ForEach(type => EfRepositoryRegistrar.RegisterRepositories(type, builder));

            return builder;
        }

        public static IIocBuilder UseTypedConnectionStringResolver(this IIocBuilder builder)
        {
            builder.RegisterServices(r => r.Register<IConnectionStringResolver, TypedConnectionStringResolver>());
            return builder;
        }
        
        public static IIocBuilder UseTransacitonScopeEfTransactionStrategy(this IIocBuilder builder)
        {
            builder.RegisterServices(r => r.Register<IEfTransactionStrategy, TransactionScopeEfTransactionStrategy>());
            return builder;
        }

        public static IIocBuilder UseDbContextEfTransactionStrategy(this IIocBuilder builder)
        {
            builder.RegisterServices(r => r.Register<IEfTransactionStrategy, DbContextEfTransactionStrategy>());
            return builder;
        }
    }
}
