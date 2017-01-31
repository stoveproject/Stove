using System;

using Autofac;
using Autofac.Extras.IocManager;

using Stove.Domain.Entities;
using Stove.EntityFramework.EntityFramework;
using Stove.Reflection.Extensions;

namespace Stove.Dapper.Dapper
{
    public static class DapperRepositoryRegistrar
    {
        public static void RegisterRepositories(Type dbContextType, IIocBuilder builder)
        {
            AutoRepositoryTypesAttribute autoRepositoryAttr = dbContextType.GetSingleAttributeOrNull<AutoRepositoryTypesAttribute>() ??
                                                              DapperAutoRepositoryTypes.Default;

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

                    builder.RegisterServices(r => r.UseBuilder(cb => cb.RegisterType(implType).As(genericRepositoryType).AsImplementedInterfaces().InjectPropertiesAsAutowired()));
                }
                else
                {
                    Type genericRepositoryTypeWithPrimaryKey = autoRepositoryAttr.RepositoryInterfaceWithPrimaryKey.MakeGenericType(entityTypeInfo.EntityType, primaryKeyType);

                    implType = autoRepositoryAttr.RepositoryImplementationWithPrimaryKey.GetGenericArguments().Length == 2
                        ? autoRepositoryAttr.RepositoryImplementationWithPrimaryKey.MakeGenericType(entityTypeInfo.EntityType, primaryKeyType)
                        : autoRepositoryAttr.RepositoryImplementationWithPrimaryKey.MakeGenericType(entityTypeInfo.DeclaringType, entityTypeInfo.EntityType, primaryKeyType);

                    builder.RegisterServices(r => r.UseBuilder(cb => cb.RegisterType(implType).As(genericRepositoryTypeWithPrimaryKey).AsImplementedInterfaces().InjectPropertiesAsAutowired()));
                }
            }
        }
    }
}
