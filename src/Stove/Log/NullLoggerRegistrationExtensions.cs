using Autofac.Extras.IocManager;

namespace Stove.Log
{
    public static class NullLoggerRegistrationExtensions
    {
        public static IIocBuilder UseNullLogger(this IIocBuilder builder)
        {
            builder.RegisterServices(r => r.Register<ILogger, NullLogger>(keepDefault: true));
            return builder;
        }
    }
}
