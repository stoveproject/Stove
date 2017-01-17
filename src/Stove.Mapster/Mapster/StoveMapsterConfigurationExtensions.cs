using Stove.Configuration;

namespace Stove.Mapster.Mapster
{
    public static class StoveMapsterConfigurationExtensions
    {
        public static IStoveMapsterConfiguraiton StoveMapster(this IModuleConfigurations configurations)
        {
            return configurations.StoveConfiguration.Get<IStoveMapsterConfiguraiton>();
        }
    }
}
