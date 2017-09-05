using System;
using System.Reflection;

using Autofac.Extras.IocManager;

using JetBrains.Annotations;

using Stove.Configurations;
using Stove.Reflection.Extensions;

namespace Stove
{
    public static class StoveHangfireRegistrationExtensions
    {
        /// <summary>
        ///     Uses the stove hangfire.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="configureAction">The configure action.</param>
        /// <returns></returns>
        [NotNull]
        public static IIocBuilder UseStoveHangfire([NotNull] this IIocBuilder builder, [NotNull] Func<IStoveHangfireConfiguration, IStoveHangfireConfiguration> configureAction)
        {
            return builder.RegisterServices(r =>
            {
                r.RegisterAssemblyByConvention(typeof(StoveHangfireRegistrationExtensions).GetAssembly());
                r.Register(context => configureAction);
            });
        }
    }
}
