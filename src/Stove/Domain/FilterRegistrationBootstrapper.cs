using Stove.Bootstrapping;
using Stove.Domain.Uow;

namespace Stove.Domain
{
    public class FilterRegistrationBootstrapper : StoveBootstrapper
    {
        public override void Start()
        {
            Configuration.UnitOfWork.RegisterFilter(StoveDataFilters.SoftDelete, true);
        }
    }
}
