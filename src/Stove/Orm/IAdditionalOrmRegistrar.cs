using Stove.Domain.Repositories;

namespace Stove.Orm
{
    public interface IAdditionalOrmRegistrar
    {
        string OrmContextKey { get; }

        void RegisterRepositories(AutoRepositoryTypesAttribute defaultRepositoryTypes);
    }
}
