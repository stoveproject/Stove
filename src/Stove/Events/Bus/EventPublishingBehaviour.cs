using System.Collections.Generic;

namespace Stove.Events.Bus
{
    public delegate void EventPublishingBehaviour(IEvent @event, Dictionary<string, object> headers);
}