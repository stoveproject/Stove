using Stove.Dapper.Dapper.Repositories;
using Stove.EntityFramework.EntityFramework;

namespace Stove.Dapper.Dapper
{
    public static class DapperAutoRepositoryTypes
    {
        static DapperAutoRepositoryTypes()
        {
            Default = new AutoRepositoryTypesAttribute(
                typeof(IDapperRepository<>),
                typeof(IDapperRepository<,>),
                typeof(DapperRepositoryBase<,>),
                typeof(DapperRepositoryBase<,,>)
            );
        }

        public static AutoRepositoryTypesAttribute Default { get; private set; }
    }
}
