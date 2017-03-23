namespace Stove.Configuration
{
    public static class StoveNHibernateConfigurationExtensions
    {
        /// <summary>
        ///     Used to configure ABP NHibernate module.
        /// </summary>
        public static IStoveNHibernateConfiguration StoveNHibernate(this IModuleConfigurations configurations)
        {
            return configurations.StoveConfiguration.Get<IStoveNHibernateConfiguration>();
        }
    }
}
