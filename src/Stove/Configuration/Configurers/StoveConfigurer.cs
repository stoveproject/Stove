using Autofac;
using Autofac.Extras.IocManager;

namespace Stove.Configuration.Configurers
{
    public abstract class StoveConfigurer : IStartable, ITransientDependency
    {
        protected StoveConfigurer(IStoveStartupConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IStoveStartupConfiguration Configuration { get; }

        public abstract void Start();
    }
}
