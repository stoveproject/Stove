using Hangfire;

namespace Stove.Hangfire
{
    public interface IStoveHangfireConfiguration
    {
        BackgroundJobServer Server { get; set; }

        IGlobalConfiguration GlobalConfiguration { get; }
    }
}
