using System.Threading.Tasks;

using JetBrains.Annotations;

using Stove.Threading.BackgrodunWorkers;

namespace Stove.BackgroundJobs
{
    public interface IScheduleJobManager : IBackgroundWorker
    {
        [NotNull]
        Task ScheduleAsync<TJob, TArgs>(TArgs args, [NotNull] string interval) where TJob : IBackgroundJob<TArgs>;
    }
}
