using Stove.Configuration.Configurers;
using Stove.Domain.Uow;

namespace Stove.Domain
{
    public class FilterRegistrationConfigurer : StoveConfigurer
    {
        public override void Start()
        {
            Configuration.UnitOfWork.RegisterFilter(StoveDataFilters.SoftDelete, true);
        }
    }
}
