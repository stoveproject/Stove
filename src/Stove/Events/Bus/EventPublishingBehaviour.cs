namespace Stove.Events.Bus
{
    public delegate void EventPublishingBehaviour(IEvent @event, EventHeaders headers);
}
