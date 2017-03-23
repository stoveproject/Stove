namespace Stove.Configuration
{
    public static class StoveNHibernateConfigurationExtensions
    {
        public static IStoveNHibernateConfiguration StoveNHibernate(this IModuleConfigurations configurations)
        {
            return configurations.StoveConfiguration.Get<IStoveNHibernateConfiguration>();
        }
    }
}
