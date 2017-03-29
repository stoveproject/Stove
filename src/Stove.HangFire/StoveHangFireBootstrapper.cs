using Autofac;

using Stove.Bootstrapping;
using Stove.Configurations;
using Stove.Hangfire;
using Stove.Threading.BackgrodunWorkers;

namespace Stove
{
    [DependsOn(
        typeof(StoveKernelBootstrapper)
    )]
    public class StoveHangFireBootstrapper : StoveBootstrapper
    {
        private readonly IBackgroundWorkerManager _backgroundWorkerManager;

        public StoveHangFireBootstrapper(IBackgroundWorkerManager backgroundWorkerManager)
        {
            _backgroundWorkerManager = backgroundWorkerManager;
        }

        public override void Start()
        {
            Configuration.Modules.StoveHangfire().Configure(configuration =>
            {
                configuration.GlobalConfiguration.UseAutofacActivator(Resolver.Resolve<ILifetimeScope>());
                Configuration.GetConfigurerIfExists<IStoveHangfireConfiguration>().Invoke(configuration);
            });

            _backgroundWorkerManager.Add(Configuration.Resolver.Resolve<HangfireBackgroundJobManager>());
            _backgroundWorkerManager.Add(Configuration.Resolver.Resolve<HangfireScheduleJobManager>());
        }
    }
}
