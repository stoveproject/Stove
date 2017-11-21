using System;

using Autofac;
using Autofac.Extras.IocManager;

using Stove.Domain.Entities;
using Stove.Domain.Repositories;
using Stove.Reflection.Extensions;

namespace Stove.NHibernate.Enrichments
{
    public static class NhRepositoryRegistrar
    {
        public static void RegisterRepositories(Type sessionContextType, IIocBuilder builder)
        {
            AutoRepositoryTypesAttribute autoRepositoryAttr = sessionContextType.GetSingleAttributeOrNull<AutoRepositoryTypesAttribute>() ??
                                                              NhAutoRepositoryTypes.Default;

            foreach (EntityTypeInfo entityTypeInfo in SessionContextHelper.GetEntityTypeInfos(sessionContextType))
            {
                Type implType;
                Type primaryKeyType = EntityHelper.GetPrimaryKeyType(entityTypeInfo.EntityType);
                if (primaryKeyType == typeof(int))
                {
                    Type genericRepositoryType = autoRepositoryAttr.RepositoryInterface.MakeGenericType(entityTypeInfo.EntityType);

                    implType = autoRepositoryAttr.RepositoryImplementation.GetGenericArguments().Length == 1
                        ? autoRepositoryAttr.RepositoryImplementation.MakeGenericType(entityTypeInfo.EntityType)
                        : autoRepositoryAttr.RepositoryImplementation.MakeGenericType(entityTypeInfo.DeclaringType, entityTypeInfo.EntityType);

                    builder.RegisterServices(r => r.UseBuilder(cb =>
                    {
                        cb.RegisterType(implType).As(genericRepositoryType).AsSelf().AsImplementedInterfaces().WithPropertyInjection();
                    }));
                }
                else
                {
                    Type genericRepositoryTypeWithPrimaryKey = autoRepositoryAttr.RepositoryInterfaceWithPrimaryKey.MakeGenericType(entityTypeInfo.EntityType, primaryKeyType);

                    implType = autoRepositoryAttr.RepositoryImplementationWithPrimaryKey.GetGenericArguments().Length == 2
                        ? autoRepositoryAttr.RepositoryImplementationWithPrimaryKey.MakeGenericType(entityTypeInfo.EntityType, primaryKeyType)
                        : autoRepositoryAttr.RepositoryImplementationWithPrimaryKey.MakeGenericType(entityTypeInfo.DeclaringType, entityTypeInfo.EntityType, primaryKeyType);

                    builder.RegisterServices(r => r.UseBuilder(cb =>
                    {
                        cb.RegisterType(implType).As(genericRepositoryTypeWithPrimaryKey).AsSelf().AsImplementedInterfaces().WithPropertyInjection();
                    }));
                }
            }
        }
    }
}
