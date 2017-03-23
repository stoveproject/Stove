using Stove.Domain.Repositories;

namespace Stove.Orm
{
    public interface IAdditionalOrmRegistrar
    {
        string OrmKey { get; }

        void RegisterRepositories(AutoRepositoryTypesAttribute defaultRepositoryTypes);
    }
}
