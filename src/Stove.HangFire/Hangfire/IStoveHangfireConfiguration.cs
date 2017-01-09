using Hangfire;

namespace Stove.Hangfire.Hangfire
{
    public interface IStoveHangfireConfiguration
    {
        BackgroundJobServer Server { get; set; }

        IGlobalConfiguration GlobalConfiguration { get; }
    }
}
