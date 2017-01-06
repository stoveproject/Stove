using Stove.Configuration;

namespace Stove.BackgroundJobs
{
    public class BackgroundJobConfiguration : IBackgroundJobConfiguration
    {
        public BackgroundJobConfiguration(IStoveStartupConfiguration stoveConfiguration)
        {
            StoveConfiguration = stoveConfiguration;
        }

        public bool IsJobExecutionEnabled { get; set; }

        public IStoveStartupConfiguration StoveConfiguration { get; }
    }
}
