using Stove.Domain.Repositories;

namespace Stove.Orm
{
    public interface ISecondaryOrmRegistrar
    {
        string OrmContextKey { get; }

        void RegisterRepositories(AutoRepositoryTypesAttribute defaultRepositoryTypes);
    }
}
