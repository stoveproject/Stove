using Autofac;

using Hangfire;
using Hangfire.Annotations;

namespace Stove.Hangfire.Hangfire
{
    public static class GlobalConfigurationExtensions
    {
        public static IGlobalConfiguration<AutofacJobActivator> UseAutofacActivator(
            [NotNull] this IGlobalConfiguration configuration,
            [NotNull] ILifetimeScope lifetimeScope, bool useTaggedLifetimeScope = true)
        {
            Check.NotNull(configuration, nameof(configuration));
            Check.NotNull(lifetimeScope, nameof(lifetimeScope));

            return configuration.UseActivator(new AutofacJobActivator(lifetimeScope, useTaggedLifetimeScope));
        }
    }
}
