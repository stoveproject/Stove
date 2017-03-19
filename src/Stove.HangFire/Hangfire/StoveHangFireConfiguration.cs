using Autofac.Extras.IocManager;

using Hangfire;

namespace Stove.Hangfire
{
    public class StoveHangfireConfiguration : IStoveHangfireConfiguration, ISingletonDependency
    {
        public BackgroundJobServer Server { get; set; }

        public IGlobalConfiguration GlobalConfiguration => global::Hangfire.GlobalConfiguration.Configuration;
    }
}
