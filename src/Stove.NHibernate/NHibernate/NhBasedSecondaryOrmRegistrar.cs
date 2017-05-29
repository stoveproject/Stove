using Autofac.Extras.IocManager;

using Stove.Domain.Repositories;
using Stove.Orm;

namespace Stove.NHibernate
{
    public class NhBasedSecondaryOrmRegistrar : ISecondaryOrmRegistrar
    {
        private readonly IIocBuilder _iocBuilder;

        public NhBasedSecondaryOrmRegistrar(IIocBuilder iocBuilder)
        {
            _iocBuilder = iocBuilder;
        }

        public void RegisterRepositories(AutoRepositoryTypesAttribute defaultRepositoryTypes)
        {
            _iocBuilder.RegisterServices(r =>
            {
                r.RegisterGeneric(defaultRepositoryTypes.RepositoryInterface, defaultRepositoryTypes.RepositoryImplementation);
                r.RegisterGeneric(defaultRepositoryTypes.RepositoryInterfaceWithPrimaryKey, defaultRepositoryTypes.RepositoryImplementationWithPrimaryKey);
            });
        }

        public string OrmContextKey => StoveConsts.Orms.NHibernate;
    }
}
