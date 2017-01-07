using Autofac;
using Autofac.Extras.IocManager;

using NLog;

using ILogger = Stove.Log.ILogger;

namespace Stove.NLog
{
    public static class NLogRegistrationExtensions
    {
        public static IIocBuilder UseNLog(this IIocBuilder builder)
        {
            builder.RegisterServices(r => r.Register<ILogger>(context => new LoggerAdapter(LogManager.GetCurrentClassLogger()), Lifetime.Singleton));
            builder.RegisterServices(r => r.UseBuilder(containerBuilder => containerBuilder.RegisterModule<NLogRegistrarModule>()));
            return builder;
        }
    }
}
