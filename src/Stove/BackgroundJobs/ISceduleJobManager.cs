using System.Threading.Tasks;

using JetBrains.Annotations;

using Stove.Threading.BackgrodunWorkers;

namespace Stove.BackgroundJobs
{
    /// <summary>
    ///     Defines an interface to Scheduling and Reccurent job operations.
    /// </summary>
    /// <seealso cref="Stove.Threading.BackgrodunWorkers.IBackgroundWorker" />
    public interface IScheduleJobManager : IBackgroundWorker
    {
        /// <summary>
        ///     Schedules a job to be executed in given interval.
        /// </summary>
        /// <typeparam name="TJob">The type of the job.</typeparam>
        /// <typeparam name="TArgs">The type of the arguments.</typeparam>
        /// <param name="args">The arguments.</param>
        /// <param name="interval">The interval.</param>
        /// <returns></returns>
        [NotNull]
        Task ScheduleAsync<TJob, TArgs>(TArgs args, [NotNull] string interval) where TJob : IBackgroundJob<TArgs>;
    }
}
