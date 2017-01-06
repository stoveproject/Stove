using Stove.Configuration;

namespace Stove.BackgroundJobs
{
    /// <summary>
    ///     Used to configure background job system.
    /// </summary>
    public interface IBackgroundJobConfiguration
    {
        /// <summary>
        ///     Used to enable/disable background job execution.
        /// </summary>
        bool IsJobExecutionEnabled { get; set; }

        /// <summary>
        ///     Gets the Stove configuration object.
        /// </summary>
        IStoveStartupConfiguration StoveConfiguration { get; }
    }
}
