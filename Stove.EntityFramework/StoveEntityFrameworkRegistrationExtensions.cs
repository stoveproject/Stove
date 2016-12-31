using System;
using System.Collections.Generic;
using System.Reflection;

using Autofac.Extras.IocManager;

using FluentAssemblyScanner;

using Stove.Domain.Entities;
using Stove.Domain.Uow;
using Stove.EntityFramework.EntityFramework;
using Stove.EntityFramework.EntityFramework.Repositories;
using Stove.EntityFramework.EntityFramework.Uow;
using Stove.Reflection.Extensions;

namespace Stove.EntityFramework
{
    public static class StoveEntityFrameworkRegistrationExtensions
    {
        public static IIocBuilder UseEntityFramework(this IIocBuilder builder)
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

            dbContextTypes.ForEach(type => RegisterRepositories(type, builder));

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

        private static void RegisterRepositories(Type dbContextType, IIocBuilder builder)
        {
            AutoRepositoryTypesAttribute autoRepositoryAttr = dbContextType.GetSingleAttributeOrNull<AutoRepositoryTypesAttribute>() ??
                                                              EfAutoRepositoryTypes.Default;

            foreach (EntityTypeInfo entityTypeInfo in DbContextHelper.GetEntityTypeInfos(dbContextType))
            {
                Type implType;
                Type primaryKeyType = EntityHelper.GetPrimaryKeyType(entityTypeInfo.EntityType);
                if (primaryKeyType == typeof(int))
                {
                    Type genericRepositoryType = autoRepositoryAttr.RepositoryInterface.MakeGenericType(entityTypeInfo.EntityType);

                    implType = autoRepositoryAttr.RepositoryImplementation.GetGenericArguments().Length == 1
                        ? autoRepositoryAttr.RepositoryImplementation.MakeGenericType(entityTypeInfo.EntityType)
                        : autoRepositoryAttr.RepositoryImplementation.MakeGenericType(entityTypeInfo.DeclaringType, entityTypeInfo.EntityType);

                    builder.RegisterServices(r => r.Register(genericRepositoryType, implType));
                }
                else
                {
                    Type genericRepositoryTypeWithPrimaryKey = autoRepositoryAttr.RepositoryInterfaceWithPrimaryKey.MakeGenericType(entityTypeInfo.EntityType, primaryKeyType);

                    implType = autoRepositoryAttr.RepositoryImplementationWithPrimaryKey.GetGenericArguments().Length == 2
                        ? autoRepositoryAttr.RepositoryImplementationWithPrimaryKey.MakeGenericType(entityTypeInfo.EntityType, primaryKeyType)
                        : autoRepositoryAttr.RepositoryImplementationWithPrimaryKey.MakeGenericType(entityTypeInfo.DeclaringType, entityTypeInfo.EntityType, primaryKeyType);

                    builder.RegisterServices(r => r.Register(genericRepositoryTypeWithPrimaryKey, implType));
                }
            }
        }
    }
}
