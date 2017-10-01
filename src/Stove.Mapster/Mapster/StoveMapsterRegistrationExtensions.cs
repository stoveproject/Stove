using Autofac.Extras.IocManager;

using JetBrains.Annotations;

using Stove.ObjectMapping;
using Stove.Reflection.Extensions;

namespace Stove.Mapster
{
    public static class StoveMapsterRegistrationExtensions
    {
        /// <summary>
        ///     Uses the stove mapster.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        [NotNull]
        public static IIocBuilder UseStoveMapster([NotNull] this IIocBuilder builder)
        {
            return builder.RegisterServices(r =>
            {
                r.RegisterAssemblyByConvention(typeof(StoveMapsterRegistrationExtensions).GetAssembly());
                r.Register<IStoveMapsterConfiguration, StoveMapsterConfiguration>(Lifetime.Singleton);
                r.Register<IObjectMapper, MapsterObjectMapper>(Lifetime.Singleton);
            });
        }
    }
}
