using System.Reflection;

using Autofac.Extras.IocManager;

using Stove.Mapster.Mapster;
using Stove.ObjectMapping;

namespace Stove.Mapster
{
    public static class StoveMapsterRegistrationExtensions
    {
        public static IIocBuilder UseStoveMapster(this IIocBuilder builder)
        {
            return builder
                .RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly()))
                .RegisterServices(r => r.Register<IStoveMapsterConfiguraiton, StoveMapsterConfiguration>(Lifetime.Singleton))
                .RegisterServices(r => r.Register<IObjectMapper, MapsterObjectMapper>(Lifetime.Singleton));
        }
    }
}
