using Stove.Events.Bus;

namespace Stove.Tests.SampleApplication.Domain.Events
{
    public class CampaignCreatedEvent : Event
    {
        public string Name { get; set; }
    }
}
