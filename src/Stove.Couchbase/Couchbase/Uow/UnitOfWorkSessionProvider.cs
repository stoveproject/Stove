using Autofac.Extras.IocManager;

using Couchbase.Linq;

using Stove.Domain.Uow;

namespace Stove.Couchbase.Couchbase.Uow
{
    public class UnitOfWorkSessionProvider : ISessionProvider, ITransientDependency
    {
        private readonly ICurrentUnitOfWorkProvider _unitOfWorkProvider;

        public UnitOfWorkSessionProvider(ICurrentUnitOfWorkProvider unitOfWorkProvider)
        {
            _unitOfWorkProvider = unitOfWorkProvider;
        }

        public IBucketContext Session => _unitOfWorkProvider.Current.GetSession();
    }
}
