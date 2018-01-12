using Stove.Events.Bus;

namespace Stove.NHibernate.Tests.Entities.Events
{
    public class ProductCreatedEvent : Event
    {
        public readonly string Name;

        public ProductCreatedEvent(string name)
        {
            Name = name;
        }
    }
}
