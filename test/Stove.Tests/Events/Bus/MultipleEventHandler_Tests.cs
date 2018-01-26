using System.Collections.Generic;

using Stove.Events.Bus;
using Stove.Events.Bus.Handlers;

using Xunit;

namespace Stove.Tests.Events.Bus
{
    public class MultipleEventHandler_Tests : EventBusTestBase
    {
        [Fact]
        public void when_multiple_event_handlers_are_registered_then_should_be_invoked_all()
        {
            EventBus.Register(new FirstHandler());
            EventBus.Register(new SecondHandler());

            EventBus.Publish(new ProductCreatedEvent(), new Dictionary<string, object>());
        }

        public class ProductCreatedEvent : Event
        {
        }

        public class FirstHandler : IEventHandler<ProductCreatedEvent>
        {
            public void Handle(ProductCreatedEvent @event, Dictionary<string, object> headers)
            {
            }
        }

        public class SecondHandler : IEventHandler<ProductCreatedEvent>
        {
            public void Handle(ProductCreatedEvent @event, Dictionary<string, object> headers)
            {
            }
        }
    }
}
