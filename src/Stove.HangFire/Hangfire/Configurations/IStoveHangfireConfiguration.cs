using Hangfire;

using Stove.Configuration;

namespace Stove.Hangfire.Configurations
{
    public interface IStoveHangfireConfiguration
    {
        BackgroundJobServer Server { get; set; }

        IGlobalConfiguration GlobalConfiguration { get; }

        IStoveStartupConfiguration StoveConfiguration { get; }
    }
}
