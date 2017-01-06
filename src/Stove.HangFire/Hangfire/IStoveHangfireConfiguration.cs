using Hangfire;

namespace Stove.HangFire.Hangfire
{
    public interface IStoveHangfireConfiguration
    {
        BackgroundJobServer Server { get; set; }

        IGlobalConfiguration GlobalConfiguration { get; }
    }
}
