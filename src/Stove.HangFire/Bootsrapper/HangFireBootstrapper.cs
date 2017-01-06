using Autofac.Extras.IocManager;

using Stove.Bootstrapping;
using Stove.HangFire.Hangfire;
using Stove.Threading.BackgrodunWorkers;

namespace Stove.HangFire.Bootsrapper
{
    public class HangFireBootstrapper : StoveBootstrapper
    {
        private readonly IBackgroundWorkerManager _backgroundWorkerManager;
        private readonly IResolver _resolver;

        public HangFireBootstrapper(IBackgroundWorkerManager backgroundWorkerManager, IResolver resolver)
        {
            _backgroundWorkerManager = backgroundWorkerManager;
            _resolver = resolver;
        }

        public override void Start()
        {
            _backgroundWorkerManager.Add(_resolver.Resolve<HangfireBackgroundJobManager>());
        }
    }
}
