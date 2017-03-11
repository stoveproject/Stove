using System;
using System.Threading.Tasks;

using Autofac.Extras.IocManager;

using Hangfire;

using Stove.BackgroundJobs;
using Stove.Threading.BackgrodunWorkers;

namespace Stove.Hangfire.Hangfire
{
    public class HangfireScheduleJobManager : BackgroundWorkerBase, IScheduleJobManager, ISingletonDependency
    {
        private readonly IBackgroundJobConfiguration _backgroundJobConfiguration;
        private readonly IStoveHangfireConfiguration _hangfireConfiguration;

        public HangfireScheduleJobManager(IStoveHangfireConfiguration hangfireConfiguration, IBackgroundJobConfiguration backgroundJobConfiguration)
        {
            _hangfireConfiguration = hangfireConfiguration;
            _backgroundJobConfiguration = backgroundJobConfiguration;
        }

        public override void Start()
        {
            base.Start();

            if (_hangfireConfiguration.Server == null && _backgroundJobConfiguration.IsJobExecutionEnabled)
            {
                _hangfireConfiguration.Server = new BackgroundJobServer();
            }
        }

        public override void WaitToStop()
        {
            if (_hangfireConfiguration.Server != null)
            {
                try
                {
                    _hangfireConfiguration.Server.Dispose();
                }
                catch (Exception ex)
                {
                    Logger.Warn(ex.ToString(), ex);
                }
            }

            base.WaitToStop();
        }

        public Task ScheduleAsync<TJob, TArgs>(TArgs args, string interval) where TJob : IBackgroundJob<TArgs>
        {
            RecurringJob.AddOrUpdate<TJob>(job => job.Execute(args), interval, TimeZoneInfo.Local);
            return Task.FromResult(0);
        }
    }
}
