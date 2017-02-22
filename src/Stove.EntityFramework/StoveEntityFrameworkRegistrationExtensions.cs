using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Autofac.Extras.IocManager;

using JetBrains.Annotations;

using Stove.Domain.Uow;
using Stove.EntityFramework.EntityFramework;
using Stove.EntityFramework.EntityFramework.Uow;
using Stove.Reflection.Extensions;

namespace Stove.EntityFramework
{
    public static class StoveEntityFrameworkRegistrationExtensions
    {
        /// <summary>
        ///     Uses the stove entity framework.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        [NotNull]
        public static IIocBuilder UseStoveEntityFramework([NotNull] this IIocBuilder builder)
        {
            builder.RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly()));
            builder.RegisterServices(r => r.Register<IUnitOfWorkFilterExecuter, IEfUnitOfWorkFilterExecuter, EfDynamicFiltersUnitOfWorkFilterExecuter>());
            builder.RegisterServices(r => r.RegisterGeneric(typeof(IDbContextProvider<>), typeof(UnitOfWorkDbContextProvider<>)));
            builder.RegisterServices(r => r.Register<IUnitOfWorkDefaultOptions, UnitOfWorkDefaultOptions>(Lifetime.Singleton));

            List<Type> dbContextTypes = typeof(StoveDbContext).AssignedTypes().ToList();
            dbContextTypes.ForEach(type => EfRepositoryRegistrar.RegisterRepositories(type, builder));

            return builder;
        }

        /// <summary>
        ///     Uses the repository registrar in assembly.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        [NotNull]
        public static IIocBuilder UseRepositoryRegistrarInAssembly([NotNull] this IIocBuilder builder, [NotNull] Assembly assembly)
        {
            Check.NotNull(assembly, nameof(assembly));

            List<Type> dbContextTypes = typeof(StoveDbContext).AssignedTypesInAssembly(assembly).ToList();
            dbContextTypes.ForEach(type => EfRepositoryRegistrar.RegisterRepositories(type, builder));
            return builder;
        }

        /// <summary>
        ///     Uses the stove typed connection string resolver.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        [NotNull]
        public static IIocBuilder UseStoveTypedConnectionStringResolver([NotNull] this IIocBuilder builder)
        {
            builder.RegisterServices(r => r.Register<IConnectionStringResolver, TypedConnectionStringResolver>());
            return builder;
        }

        /// <summary>
        ///     Uses the stove transaction scope ef transaction strategy.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        [NotNull]
        public static IIocBuilder UseStoveTransactionScopeEfTransactionStrategy([NotNull] this IIocBuilder builder)
        {
            builder.RegisterServices(r => r.Register<IEfTransactionStrategy, TransactionScopeEfTransactionStrategy>());
            return builder;
        }

        /// <summary>
        ///     Uses the stove database context ef transaction strategy.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        [NotNull]
        public static IIocBuilder UseStoveDbContextEfTransactionStrategy([NotNull] this IIocBuilder builder)
        {
            builder.RegisterServices(r => r.Register<IEfTransactionStrategy, DbContextEfTransactionStrategy>());
            return builder;
        }
    }
}
