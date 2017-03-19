using Stove.Dapper.Repositories;

namespace Stove.Dapper
{
    public static class DapperAutoRepositoryTypes
    {
        static DapperAutoRepositoryTypes()
        {
            Default = new DapperAutoRepositoryTypeAttribute(
                typeof(IDapperRepository<>),
                typeof(IDapperRepository<,>),
                typeof(DapperRepositoryBase<,>),
                typeof(DapperRepositoryBase<,,>)
            );
        }

        public static DapperAutoRepositoryTypeAttribute Default { get; private set; }
    }
}
