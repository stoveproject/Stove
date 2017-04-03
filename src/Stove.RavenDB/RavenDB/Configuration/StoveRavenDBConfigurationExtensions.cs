using Stove.Configuration;

namespace Stove.RavenDB.Configuration
{
    public static class StoveRavenDBConfigurationExtensions
    {
        public static IStoveRavenDBConfiguration StoveRavenDB(this IModuleConfigurations modules)
        {
            return modules.StoveConfiguration.Get<IStoveRavenDBConfiguration>();
        }
    }
}
