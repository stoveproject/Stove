using System.Collections.Generic;

namespace Stove.Events.Bus.Entities
{
    public class AggregateChangeReport
    {
        public AggregateChangeReport()
        {
            DomainEvents = new List<DomainEvent>();
        }

        public List<DomainEvent> DomainEvents { get; }

        public bool IsEmpty()
        {
            return DomainEvents.Count <= 0;
        }

        public override string ToString()
        {
            return $"[AggregateChangeReport] DomainEvents: {DomainEvents.Count}";
        }
    }
}
