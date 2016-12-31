using System.Collections.Generic;

using Stove.Events.Bus;

namespace Stove.Domain.Entities
{
    public interface IAggregateRoot : IAggregateRoot<int>, IEntity
    {

    }

    public interface IAggregateRoot<TPrimaryKey> : IEntity<TPrimaryKey>, IGeneratesDomainEvents
    {

    }

    public interface IGeneratesDomainEvents
    {
        ICollection<IEventData> DomainEvents { get; }
    }
}