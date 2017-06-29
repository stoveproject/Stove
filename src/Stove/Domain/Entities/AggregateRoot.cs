using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

using Stove.Events.Bus;

namespace Stove.Domain.Entities
{
    public class AggregateRoot : AggregateRoot<int>, IAggregateRoot
    {
    }

    public class AggregateRoot<TPrimaryKey> : Entity<TPrimaryKey>, IAggregateRoot<TPrimaryKey>
    {
        public AggregateRoot()
        {
            DomainEvents = new Collection<IEventData>();
        }

        [NotMapped]
        public virtual ICollection<IEventData> DomainEvents { get; }

        protected void Raise(IEventData @event)
        {
            DomainEvents.Add(@event);
        }
    }
}
