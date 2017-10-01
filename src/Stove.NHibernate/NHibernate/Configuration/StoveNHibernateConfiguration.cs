using Autofac.Extras.IocManager;

using FluentNHibernate.Cfg;

using Stove.Configuration;

namespace Stove.NHibernate.Configuration
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
