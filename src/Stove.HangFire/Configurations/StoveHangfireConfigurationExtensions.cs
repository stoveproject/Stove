using System;

using JetBrains.Annotations;

using Stove.Configuration;
using Stove.Hangfire.Hangfire;

namespace Stove.Hangfire.Configurations
{
    public static class StoveHangfireConfigurationExtensions
    {
        [NotNull]
        public static IStoveHangfireConfiguration StoveHangfire([NotNull] this IModuleConfigurations configurations)
        {
            return configurations.StoveConfiguration.Get<IStoveHangfireConfiguration>();
        }

        public static void Configure([NotNull] this IStoveHangfireConfiguration configuration, [NotNull] Action<IStoveHangfireConfiguration> configureAction)
        {
            configureAction(configuration);
        }
    }
}
