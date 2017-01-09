using System.Reflection;

using Autofac.Extras.IocManager;

namespace Stove.Demo.ConsoleApp
{
    public static class SomeRegistrationExtensions
    {
        public static IIocBuilder UseSomeFeature(this IIocBuilder builder)
        {
            builder.RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly()));
            return builder;
        }
    }
}
