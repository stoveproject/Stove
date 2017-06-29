using Stove.Events.Bus;

namespace Stove.Tests.SampleApplication.Domain.Events
{
    public class CampaignCreatedEvent : EventData
    {
        public string Name { get; set; }
    }
}
