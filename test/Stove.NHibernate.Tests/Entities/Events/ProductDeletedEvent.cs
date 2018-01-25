using Stove.Events.Bus;

namespace Stove.NHibernate.Tests.Entities.Events
{
    public class ProductDeletedEvent : Event
    {
        public readonly int Id;
        public readonly string Name;

        public ProductDeletedEvent(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
