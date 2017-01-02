using Stove.Configuration;
using Stove.Configuration.Configurers;
using Stove.Domain.Uow;

namespace Stove.Domain
{
    public class FilterRegistrationConfigurer : StoveConfigurer
    {
        public FilterRegistrationConfigurer(IStoveStartupConfiguration configuration) : base(configuration)
        {
        }

        public override void Start()
        {
            Configuration.UnitOfWork.RegisterFilter(StoveDataFilters.SoftDelete, true);
        }
    }
}
