using Autofac.Extras.IocManager;

using FluentNHibernate.Cfg;

namespace Stove.Configuration
{
    public class StoveNHibernateConfiguration : IStoveNHibernateConfiguration, ISingletonDependency
    {
        public StoveNHibernateConfiguration(IStoveStartupConfiguration configuration)
        {
            Configuration = configuration;
            FluentConfiguration = Fluently.Configure();
        }

        public FluentConfiguration FluentConfiguration { get; }

        public IStoveStartupConfiguration Configuration { get; }
    }
}
