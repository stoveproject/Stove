using Autofac;
using Autofac.Extras.IocManager;

using Stove.Configuration;

namespace Stove.Bootstrapping
{
    public abstract class StoveBootstrapper : IStartable, IBootsrapper, ITransientDependency
    {
        public IStoveStartupConfiguration Configuration { get; set; }

        public abstract void Start();
    }
}
