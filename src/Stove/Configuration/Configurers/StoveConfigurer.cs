using Autofac;
using Autofac.Extras.IocManager;

namespace Stove.Configuration.Configurers
{
    public abstract class StoveConfigurer : IStartable, ITransientDependency
    {
        public IStoveStartupConfiguration Configuration { get; set; }

        public abstract void Start();
    }
}
