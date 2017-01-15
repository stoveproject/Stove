using System;

using Stove.BackgroundJobs;
using Stove.Threading.BackgrodunWorkers;

namespace Stove.Bootstrapping.Bootstrappers
{
    [DependsOn(
        typeof(StoveKernelBootstrapper)
        )]
    public class BackgroundWorkerBootstrapper : StoveBootstrapper
    {
        private readonly Func<IBackgroundJobConfiguration, IBackgroundJobConfiguration> _backgroundJobConfigurer;
        private readonly IBackgroundWorkerManager _backgroundWorkerManager;

        public BackgroundWorkerBootstrapper(
            IBackgroundWorkerManager backgroundWorkerManager,
            Func<IBackgroundJobConfiguration, IBackgroundJobConfiguration> backgroundJobConfigurer)
        {
            _backgroundWorkerManager = backgroundWorkerManager;
            _backgroundJobConfigurer = backgroundJobConfigurer;
        }

        public override void Start()
        {
            _backgroundJobConfigurer(Configuration.BackgroundJobs);

            if (Configuration.BackgroundJobs.IsJobExecutionEnabled)
            {
                _backgroundWorkerManager.Start();
            }
        }
    }
}
