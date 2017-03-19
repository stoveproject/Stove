using System.Reflection;

using Autofac;
using Autofac.Extras.IocManager;

using JetBrains.Annotations;

using NLog;

using ILogger = Stove.Log.ILogger;

namespace Stove
{
    public static class StoveNLogRegistrationExtensions
    {
        /// <summary>
        ///     Uses the stove n log.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        [NotNull]
        public static IIocBuilder UseStoveNLog([NotNull] this IIocBuilder builder)
        {
            builder.RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly()));
            builder.RegisterServices(r => r.Register<ILogger>(context => new LoggerAdapter(LogManager.GetCurrentClassLogger()), Lifetime.Singleton));
            builder.RegisterServices(r => r.UseBuilder(containerBuilder => containerBuilder.RegisterModule<NLogRegistrarModule>()));
            return builder;
        }
    }
}
