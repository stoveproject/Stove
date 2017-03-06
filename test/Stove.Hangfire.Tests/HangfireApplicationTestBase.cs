using Hangfire.MemoryStorage;

using Stove.TestBase;

namespace Stove.Hangfire.Tests
{
    public class HangfireApplicationTestBase : ApplicationTestBase<StoveHangFireBootstrapper>
    {
        public HangfireApplicationTestBase()
        {
            Building(builder =>
            {
                builder
                    .UseStoveBackgroundJobs()
                    .UseStoveHangfire(configuration =>
                    {
                        configuration.GlobalConfiguration.UseMemoryStorage();
                        return configuration;
                    });
            });
        }
    }
}
