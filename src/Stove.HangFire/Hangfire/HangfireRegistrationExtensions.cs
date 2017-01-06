using System;
using System.Reflection;

using Autofac.Extras.IocManager;

using Hangfire;

namespace Stove.HangFire.Hangfire
{
    public static class HangfireRegistrationExtensions
    {
        public static IIocBuilder UseHangfire(this IIocBuilder builder, Func<IStoveHangfireConfiguration, IStoveHangfireConfiguration> configureAction)
        {
            builder.RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly()));
            builder.RegisterServices(r => r.Register(context => configureAction));
            return builder;
        }
    }
}
