namespace Stove.Events.Bus.Entities
{
    public class DomainEventEntry
    {
        public object SourceEntity { get; }

        public IEvent Event { get; }

        public DomainEventEntry(object sourceEntity, IEvent @event)
        {
            SourceEntity = sourceEntity;
            Event = @event;
        }
    }
}