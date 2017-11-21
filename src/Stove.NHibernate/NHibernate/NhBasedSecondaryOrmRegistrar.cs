using System;
using System.Collections.Generic;

using Autofac;
using Autofac.Extras.IocManager;

using Stove.Domain.Entities;
using Stove.Domain.Repositories;
using Stove.Orm;

namespace Stove.NHibernate
{
    public class NhBasedSecondaryOrmRegistrar : ISecondaryOrmRegistrar
    {
        private readonly Type _stoveSessionContextType;
        private readonly Func<Type, IEnumerable<EntityTypeInfo>> _entityTypeInfoFinder;
        private readonly IIocBuilder _iocBuilder;
        private readonly Func<Type, Type> _primaryKeyTypeFinder;

        public NhBasedSecondaryOrmRegistrar(
            IIocBuilder iocBuilder,
            Type stoveSessionContextType,
            Func<Type, IEnumerable<EntityTypeInfo>> entityTypeInfoFinder,
            Func<Type, Type> primaryKeyTypeFinder)
        {
            _stoveSessionContextType = stoveSessionContextType;
            _entityTypeInfoFinder = entityTypeInfoFinder;
            _primaryKeyTypeFinder = primaryKeyTypeFinder;
            _iocBuilder = iocBuilder;
        }

        public void RegisterRepositories(AutoRepositoryTypesAttribute defaultRepositoryTypes)
        {
            AutoRepositoryTypesAttribute autoRepositoryAttr = defaultRepositoryTypes;

            foreach (EntityTypeInfo entityTypeInfo in _entityTypeInfoFinder(_stoveSessionContextType))
            {
                Type implType;
                Type primaryKeyType = _primaryKeyTypeFinder(entityTypeInfo.EntityType);
                if (primaryKeyType == typeof(int))
                {
                    Type genericRepositoryType = autoRepositoryAttr.RepositoryInterface.MakeGenericType(entityTypeInfo.EntityType);

                    implType = autoRepositoryAttr.RepositoryImplementation.GetGenericArguments().Length == 1
                        ? autoRepositoryAttr.RepositoryImplementation.MakeGenericType(entityTypeInfo.EntityType)
                        : autoRepositoryAttr.RepositoryImplementation.MakeGenericType(entityTypeInfo.DeclaringType, entityTypeInfo.EntityType);

                    _iocBuilder.RegisterServices(r => r.UseBuilder(cb =>
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


                    _iocBuilder.RegisterServices(r => r.UseBuilder(cb =>
                    {
                        cb.RegisterType(implType).As(genericRepositoryTypeWithPrimaryKey).AsSelf().AsImplementedInterfaces().WithPropertyInjection();
                    }));
                }
            }
        }

        public string OrmContextKey => StoveConsts.Orms.NHibernate;
    }
}
