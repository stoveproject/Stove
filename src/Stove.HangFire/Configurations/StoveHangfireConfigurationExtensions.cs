using System;

using Stove.Configuration;
using Stove.HangFire.Hangfire;

namespace Stove.HangFire.Configurations
{
    public static class StoveHangfireConfigurationExtensions
    {
        public static IStoveHangfireConfiguration StoveHangfire(this IModuleConfigurations configurations)
        {
            return configurations.StoveConfiguration.Get<IStoveHangfireConfiguration>();
        }

        public static void Configure(this IStoveHangfireConfiguration configuration, Action<IStoveHangfireConfiguration> configureAction)
        {
            configureAction(configuration);
        }
    }
}
