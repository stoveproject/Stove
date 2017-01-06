using System;
using System.Threading.Tasks;

using Hangfire;

using Stove.BackgroundJobs;
using Stove.Threading.BackgrodunWorkers;

namespace Stove.HangFire.Hangfire
{
    public class HangfireBackgroundJobManager : BackgroundWorkerBase, IBackgroundJobManager
    {
        private readonly IStoveHangfireConfiguration _hangfireConfiguration;

        public HangfireBackgroundJobManager(IStoveHangfireConfiguration hangfireConfiguration)
        {
            _hangfireConfiguration = hangfireConfiguration;
        }

        public override void Start()
        {
            base.Start();

            if (_hangfireConfiguration.Server == null)
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

        public Task EnqueueAsync<TJob, TArgs>(TArgs args, BackgroundJobPriority priority = BackgroundJobPriority.Normal,
            TimeSpan? delay = null) where TJob : IBackgroundJob<TArgs>
        {
            if (!delay.HasValue)
            {
                BackgroundJob.Enqueue<TJob>(job => job.Execute(args));
            }
            else
            {
                BackgroundJob.Schedule<TJob>(job => job.Execute(args), delay.Value);
            }
            return Task.FromResult(0);
        }
    }
}
