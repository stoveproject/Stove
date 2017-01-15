using Autofac;
using Autofac.Extras.IocManager;

namespace Stove.Bootstrapping
{
    public class StoveMainBootstrapper : IStartable, ITransientDependency
    {
        private readonly IBootstrapperManager _bootstrapperManager;

        public StoveMainBootstrapper(IBootstrapperManager bootstrapperManager)
        {
            _bootstrapperManager = bootstrapperManager;
        }

        public void Start()
        {
            _bootstrapperManager.StartBootstrappers();
        }
    }
}