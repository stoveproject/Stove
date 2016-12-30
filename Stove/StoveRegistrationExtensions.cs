using Autofac.Extras.IocManager;

namespace Stove
{
    public static class StoveRegistrationExtensions
    {
        public static IIocBuilder UseStove(this IIocBuilder builder)
        {
            return builder;
        }
    }
}