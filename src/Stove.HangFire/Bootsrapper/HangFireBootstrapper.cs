using System;

using Autofac;
using Autofac.Extras.IocManager;

using Hangfire;

using Stove.Bootstrapping;
using Stove.HangFire.Hangfire;
using Stove.Threading.BackgrodunWorkers;

namespace Stove.HangFire.Bootsrapper
{
    public class HangFireBootstrapper : StoveBootstrapper
    {
        private readonly IBackgroundWorkerManager _backgroundWorkerManager;
        private readonly IStoveHangfireConfiguration _hangfireConfiguration;
        private readonly Func<IStoveHangfireConfiguration, IStoveHangfireConfiguration> _hangFireConfigurer;
        private readonly IResolver _resolver;

        public HangFireBootstrapper(
            IBackgroundWorkerManager backgroundWorkerManager,
            IResolver resolver,
            IStoveHangfireConfiguration hangfireConfiguration,
            Func<IStoveHangfireConfiguration, IStoveHangfireConfiguration> hangFireConfigurer)
        {
            _backgroundWorkerManager = backgroundWorkerManager;
            _resolver = resolver;
            _hangfireConfiguration = hangfireConfiguration;
            _hangFireConfigurer = hangFireConfigurer;
        }

        public override void Start()
        {
            _hangfireConfiguration.GlobalConfiguration.UseAutofacActivator(_resolver.Resolve<ILifetimeScope>());
            _hangFireConfigurer(_hangfireConfiguration);
            _backgroundWorkerManager.Add(_resolver.Resolve<HangfireBackgroundJobManager>());
        }
    }
}
