using JetBrains.Annotations;

using Stove.Configuration;

namespace Stove.Mapster.Mapster
{
    public static class StoveMapsterConfigurationExtensions
    {
        /// <summary>
        ///     Stove Mapster Configuration
        /// </summary>
        /// <param name="configurations">The configurations.</param>
        /// <returns></returns>
        [NotNull]
        public static IStoveMapsterConfiguration StoveMapster([NotNull] this IModuleConfigurations configurations)
        {
            return configurations.StoveConfiguration.Get<IStoveMapsterConfiguration>();
        }
    }
}
