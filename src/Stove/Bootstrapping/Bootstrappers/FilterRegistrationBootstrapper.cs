using Stove.Domain.Uow;

namespace Stove.Bootstrapping.Bootstrappers
{
    [DependsOn(
        typeof(StoveKernelBootstrapper)
    )]
    public class FilterRegistrationBootstrapper : StoveBootstrapper
    {
        public override void Start()
        {
            Configuration.UnitOfWork.RegisterFilter(StoveDataFilters.SoftDelete, true);
        }
    }
}
