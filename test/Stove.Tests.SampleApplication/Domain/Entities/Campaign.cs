using Stove.Domain.Entities;
using Stove.Tests.SampleApplication.Domain.Events;

namespace Stove.Tests.SampleApplication.Domain.Entities
{
    public class Campaign : AggregateRoot
    {
        public Campaign(string name)
        {
            Name = name;

            ApplyChange(new CampaignCreatedEvent { Name = Name });
        }

        public string Name { get; set; }

        public void Apply(CampaignCreatedEvent @event)
        {
            Name = @event.Name;
        }
    }
}
