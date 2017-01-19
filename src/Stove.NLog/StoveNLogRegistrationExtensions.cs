using System.Reflection;

using Autofac;
using Autofac.Extras.IocManager;

using NLog;

using ILogger = Stove.Log.ILogger;

namespace Stove.NLog
{
    public static class StoveNLogRegistrationExtensions
    {
        public static IIocBuilder UseStoveNLog(this IIocBuilder builder)
        {
            builder.RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly()));
            builder.RegisterServices(r => r.Register<ILogger>(context => new LoggerAdapter(LogManager.GetCurrentClassLogger()), Lifetime.Singleton));
            builder.RegisterServices(r => r.UseBuilder(containerBuilder => containerBuilder.RegisterModule<NLogRegistrarModule>()));
            return builder;
        }
    }
}
