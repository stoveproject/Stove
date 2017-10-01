using Stove.Configuration;

namespace Stove.NHibernate.Configuration
{
    public static class StoveNHibernateConfigurationExtensions
    {
        public static IStoveNHibernateConfiguration StoveNHibernate(this IModuleConfigurations configurations)
        {
            return configurations.StoveConfiguration.Get<IStoveNHibernateConfiguration>();
        }
    }
}
