using System;
using System.Reflection;

using Autofac.Extras.IocManager;

using Stove.Hangfire.Hangfire;

namespace Stove.Hangfire
{
    public static class StoveHangfireRegistrationExtensions
    {
        public static IIocBuilder UseStoveHangfire(this IIocBuilder builder, Func<IStoveHangfireConfiguration, IStoveHangfireConfiguration> configureAction)
        {
            builder.RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly()));
            builder.RegisterServices(r => r.Register(context => configureAction));
            return builder;
        }
    }
}
