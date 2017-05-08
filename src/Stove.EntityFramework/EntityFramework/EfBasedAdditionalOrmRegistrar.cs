using System;
using System.Collections.Generic;

using Autofac;
using Autofac.Extras.IocManager;

using Stove.Domain.Entities;
using Stove.Domain.Repositories;
using Stove.Orm;
using Stove.Reflection.Extensions;

namespace Stove.EntityFramework
{
    public class EfBasedAdditionalOrmRegistrar : IAdditionalOrmRegistrar
    {
        private readonly Type _dbContextType;
        private readonly Func<Type, IEnumerable<EntityTypeInfo>> _entityTypeInfoFinder;
        private readonly IIocBuilder _iocBuilder;
        private readonly Func<Type, Type> _primaryKeyTypeFinder;

        public EfBasedAdditionalOrmRegistrar(
            IIocBuilder iocBuilder,
            Type dbContextType,
            Func<Type, IEnumerable<EntityTypeInfo>> entityTypeInfoFinder,
            Func<Type, Type> primaryKeyTypeFinder)
        {
            _dbContextType = dbContextType;
            _entityTypeInfoFinder = entityTypeInfoFinder;
            _primaryKeyTypeFinder = primaryKeyTypeFinder;
            _iocBuilder = iocBuilder;
        }

        public void RegisterRepositories(AutoRepositoryTypesAttribute defaultRepositoryTypes)
        {
            AutoRepositoryTypesAttribute autoRepositoryAttr = _dbContextType.GetSingleAttributeOrNull<AutoRepositoryTypesAttribute>() ??
                                                              defaultRepositoryTypes;

            foreach (EntityTypeInfo entityTypeInfo in _entityTypeInfoFinder(_dbContextType))
            {
                Type implType;
                Type primaryKeyType = _primaryKeyTypeFinder(entityTypeInfo.EntityType);
                if (primaryKeyType == typeof(int))
                {
                    Type genericRepositoryType = autoRepositoryAttr.RepositoryInterface.MakeGenericType(entityTypeInfo.EntityType);

                    implType = autoRepositoryAttr.RepositoryImplementation.GetGenericArguments().Length == 1
                        ? autoRepositoryAttr.RepositoryImplementation.MakeGenericType(entityTypeInfo.EntityType)
                        : autoRepositoryAttr.RepositoryImplementation.MakeGenericType(entityTypeInfo.DeclaringType, entityTypeInfo.EntityType);

                    _iocBuilder.RegisterServices(r => r.UseBuilder(cb => cb.RegisterType(implType).As(genericRepositoryType).AsImplementedInterfaces().WithPropertyInjection()));
                }
                else
                {
                    Type genericRepositoryTypeWithPrimaryKey = autoRepositoryAttr.RepositoryInterfaceWithPrimaryKey.MakeGenericType(entityTypeInfo.EntityType, primaryKeyType);

                    implType = autoRepositoryAttr.RepositoryImplementationWithPrimaryKey.GetGenericArguments().Length == 2
                        ? autoRepositoryAttr.RepositoryImplementationWithPrimaryKey.MakeGenericType(entityTypeInfo.EntityType, primaryKeyType)
                        : autoRepositoryAttr.RepositoryImplementationWithPrimaryKey.MakeGenericType(entityTypeInfo.DeclaringType, entityTypeInfo.EntityType, primaryKeyType);

                    _iocBuilder.RegisterServices(r => r.UseBuilder(cb => cb.RegisterType(implType).As(genericRepositoryTypeWithPrimaryKey).AsImplementedInterfaces().WithPropertyInjection()));
                }
            }
        }

        public string OrmContextKey { get { return StoveOrms.EntityFramework; } }
    }
}
