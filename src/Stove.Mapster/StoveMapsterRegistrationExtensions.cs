using System.Reflection;

using Autofac.Extras.IocManager;

using JetBrains.Annotations;

using Stove.Mapster;
using Stove.ObjectMapping;

namespace Stove
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
            return builder
                .RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly()))
                .RegisterServices(r => r.Register<IStoveMapsterConfiguration, StoveMapsterConfiguration>(Lifetime.Singleton))
                .RegisterServices(r => r.Register<IObjectMapper, MapsterObjectMapper>(Lifetime.Singleton));
        }
    }
}
