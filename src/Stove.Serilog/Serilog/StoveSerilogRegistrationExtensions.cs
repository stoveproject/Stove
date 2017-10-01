using System;

using Autofac;
using Autofac.Extras.IocManager;

using Serilog;

using Stove.Reflection.Extensions;

namespace Stove.Serilog
{
	public static class StoveSerilogRegistrationExtensions
	{
		public static IIocBuilder UseStoveSerilog(this IIocBuilder builder, Action<LoggerConfiguration> configureCallback)
		{
			return builder.RegisterServices(r =>
			{
				var configuration = new LoggerConfiguration();
				configureCallback(configuration);

				r.RegisterAssemblyByConvention(typeof(StoveSerilogRegistrationExtensions).GetAssembly());
				r.Register<Log.ILogger>(ctx => new StoveSerilogLogger(ctx.Resolver.Resolve<ILogger>()), Lifetime.Singleton);
				r.Register<ILogger>(context => configuration.CreateLogger(), Lifetime.Singleton);
				r.UseBuilder(containerBuilder => containerBuilder.RegisterModule<SerilogRegistrarModule>());
			});
		}
	}
}
