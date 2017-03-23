using Autofac.Extras.IocManager;

using Stove.Domain.Repositories;
using Stove.Orm;

namespace Stove.NHibernate
{
    public class NhBasedAdditionalOrmRegistrar : IAdditionalOrmRegistrar
    {
        private readonly IIocBuilder _iocBuilder;

        public NhBasedAdditionalOrmRegistrar(IIocBuilder iocBuilder)
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

        public string OrmKey
        {
            get { return StoveOrms.NHibernate; }
        }
    }
}
