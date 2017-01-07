using Stove.Domain.Uow;

namespace Stove.Bootstrapping.Bootstrappers
{
    public class FilterRegistrationBootstrapper : StoveBootstrapper
    {
        public override void Start()
        {
            Configuration.UnitOfWork.RegisterFilter(StoveDataFilters.SoftDelete, true);
        }
    }
}
