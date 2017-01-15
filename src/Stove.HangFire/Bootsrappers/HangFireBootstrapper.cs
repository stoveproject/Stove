using System;

using Autofac;

using Hangfire;

using Stove.Bootstrapping;
using Stove.Bootstrapping.Bootstrappers;
using Stove.Hangfire.Configurations;
using Stove.Hangfire.Hangfire;
using Stove.Threading.BackgrodunWorkers;

namespace Stove.Hangfire.Bootsrappers
{
    [DependsOn(
        typeof(BackgroundWorkerBootstrapper)
    )]
    public class HangFireBootstrapper : StoveBootstrapper
    {
        private readonly IBackgroundWorkerManager _backgroundWorkerManager;
        private readonly Func<IStoveHangfireConfiguration, IStoveHangfireConfiguration> _hangFireConfigurer;

        public HangFireBootstrapper(
            IBackgroundWorkerManager backgroundWorkerManager,
            Func<IStoveHangfireConfiguration, IStoveHangfireConfiguration> hangFireConfigurer)
        {
            _backgroundWorkerManager = backgroundWorkerManager;
            _hangFireConfigurer = hangFireConfigurer;
        }

        public override void Start()
        {
            Configuration.Modules.StoveHangfire().Configure(configuration =>
            {
                configuration.GlobalConfiguration.UseAutofacActivator(Configuration.Resolver.Resolve<ILifetimeScope>());
                _hangFireConfigurer(configuration);
            });

            _backgroundWorkerManager.Add(Configuration.Resolver.Resolve<HangfireBackgroundJobManager>());
        }
    }
}
