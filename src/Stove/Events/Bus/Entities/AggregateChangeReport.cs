using System.Collections.Generic;
using System.Linq;

namespace Stove.Events.Bus.Entities
{
    public class AggregateChangeReport
    {
        public AggregateChangeReport()
        {
            DomainEvents = new List<Envelope>();
        }

        public List<Envelope> DomainEvents { get; }

        public bool IsEmpty()
        {
            return !DomainEvents.Any();
        }

        public override string ToString()
        {
            return $"[AggregateChangeReport] DomainEvents: {DomainEvents.Count}";
        }
    }
}
