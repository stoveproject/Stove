using System;

using Stove.BackgroundJobs;
using Stove.Bootstrapping;

namespace Stove.Threading.BackgrodunWorkers
{
    public class BackgroundWorkerBootstrapper : StoveBootstrapper
    {
        private readonly IBackgroundJobConfiguration _backgroundJobConfiguration;
        private readonly IBackgroundWorkerManager _backgroundWorkerManager;
        private readonly Func<IBackgroundJobConfiguration, IBackgroundJobConfiguration> _backgroundJobConfigurer;

        public BackgroundWorkerBootstrapper(IBackgroundWorkerManager backgroundWorkerManager,
            Func<IBackgroundJobConfiguration, IBackgroundJobConfiguration> backgroundJobConfigurer,
            IBackgroundJobConfiguration backgroundJobConfiguration)
        {
            _backgroundWorkerManager = backgroundWorkerManager;
            _backgroundJobConfigurer = backgroundJobConfigurer;
            _backgroundJobConfiguration = backgroundJobConfiguration;
        }

        public override void Start()
        {
            _backgroundJobConfigurer(_backgroundJobConfiguration);
            _backgroundWorkerManager.Start();
        }
    }
}
