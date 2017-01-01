using Autofac;
using Autofac.Extras.IocManager;

using Stove.Configuration;
using Stove.Domain.Uow;

namespace Stove.Startables
{
    public class StoveConfigurer : IStartable, ITransientDependency
    {
        private readonly IStoveStartupConfiguration _configuration;

        public StoveConfigurer(IStoveStartupConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Start()
        {
            _configuration.UnitOfWork.RegisterFilter(StoveDataFilters.SoftDelete, true);
        }
    }
}
