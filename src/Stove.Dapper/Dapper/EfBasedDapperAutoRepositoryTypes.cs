using Stove.Dapper.Repositories;

namespace Stove.Dapper
{
    public static class EfBasedDapperAutoRepositoryTypes
    {
        static EfBasedDapperAutoRepositoryTypes()
        {
            Default = new DapperAutoRepositoryTypeAttribute(
                typeof(IDapperRepository<>),
                typeof(IDapperRepository<,>),
                typeof(DapperRepositoryIntBase<,>),
                typeof(DapperRepositoryBase<,,>)
            );
        }

        public static DapperAutoRepositoryTypeAttribute Default { get; private set; }
    }
}
