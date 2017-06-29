using Stove.Domain.Entities;
using Stove.Tests.SampleApplication.Domain.Events;

namespace Stove.Tests.SampleApplication.Domain.Entities
{
    public class Campaign : AggregateRoot
    {
        public Campaign(string name)
        {
            Name = name;

            Raise(new CampaignCreatedEvent { Name = Name });
        }

        public string Name { get; set; }
    }
}
