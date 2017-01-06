using System;
using System.Reflection;

using Autofac.Extras.IocManager;

namespace Stove.HangFire.Hangfire
{
    public static class HangfireRegistrationExtensions
    {
        public static IIocBuilder UseHangfire(this IIocBuilder builder)
        {
            builder.RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly()));
            return builder;
        }

        public static IIocBuilder UseHangfire(this IIocBuilder builder, Action<IStoveHangfireConfiguration> configureAction)
        {
            UseHangfire(builder);
            builder.RegisterServices(r => r.Register(ctx =>
            {
                var configuration = ctx.Resolver.Resolve<IStoveHangfireConfiguration>();
                configureAction(configuration);
                return configuration;
            }));
            return builder;
        }
    }
}
