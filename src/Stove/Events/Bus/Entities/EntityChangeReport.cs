using System.Collections.Generic;

namespace Stove.Events.Bus.Entities
{
    public class EntityChangeReport
    {
        public EntityChangeReport()
        {
            DomainEvents = new List<DomainEventEntry>();
        }

        public List<DomainEventEntry> DomainEvents { get; }

        public bool IsEmpty()
        {
            return DomainEvents.Count <= 0;
        }

        public override string ToString()
        {
            return $"[EntityChangeReport] DomainEvents: {DomainEvents.Count}";
        }
    }
}
