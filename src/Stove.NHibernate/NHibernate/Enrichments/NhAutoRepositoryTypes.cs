using Stove.Domain.Repositories;
using Stove.NHibernate.Repositories;

namespace Stove.NHibernate.Enrichments
{
    public class NhAutoRepositoryTypes
    {
        static NhAutoRepositoryTypes()
        {
            Default = new AutoRepositoryTypesAttribute(
                typeof(IRepository<>),
                typeof(IRepository<,>),
                typeof(NhRepositoryBase<,>),
                typeof(NhRepositoryBase<,,>)
            );
        }

        public static AutoRepositoryTypesAttribute Default { get; }
    }
}
