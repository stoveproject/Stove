using System;

using Autofac;

using Stove.Bootstrapping;
using Stove.Hangfire.Configurations;
using Stove.Hangfire.Hangfire;
using Stove.Threading.BackgrodunWorkers;

namespace Stove.Hangfire
{
    [DependsOn(
        typeof(StoveKernelBootstrapper)
    )]
    public class StoveHangFireBootstrapper : StoveBootstrapper
    {
        private readonly IBackgroundWorkerManager _backgroundWorkerManager;
        private readonly Func<IStoveHangfireConfiguration, IStoveHangfireConfiguration> _hangFireConfigurer;

        public StoveHangFireBootstrapper(
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
                configuration.GlobalConfiguration.UseAutofacActivator(Resolver.Resolve<ILifetimeScope>());
                _hangFireConfigurer(configuration);
            });

            _backgroundWorkerManager.Add(Configuration.Resolver.Resolve<HangfireBackgroundJobManager>());
        }
    }
}
