using Autofac.Extras.IocManager;

using FluentNHibernate.Cfg;

namespace Stove.Configuration
{
    public class StoveNHibernateConfiguration : IStoveNHibernateConfiguration, ISingletonDependency
    {
        public StoveNHibernateConfiguration()
        {
            FluentConfiguration = Fluently.Configure();
        }

        public FluentConfiguration FluentConfiguration { get; }
    }
}
