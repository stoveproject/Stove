using Shouldly;

using Stove.Domain.Entities;
using Stove.Events.Bus.Entities;
using Stove.Events.Bus.Handlers;

using Xunit;

namespace Stove.Tests.Events.Bus
{
    public class EventBus_EntityEvents_Test : EventBusTestBase
    {
        [Fact]
        public void Should_Call_Created_And_Changed_Once()
        {
            var handler = new MyEventHandler();

            EventBus.Register<EntityChangedEvent<MyEntity>>(handler);
            EventBus.Register<EntityCreatedEvent<MyEntity>>(handler);

            EventBus.Publish(new EntityCreatedEvent<MyEntity>(new MyEntity()));

            handler.EntityCreatedEventCount.ShouldBe(1);
            handler.EntityChangedEventCount.ShouldBe(1);
        }

        public class MyEntity : Entity
        {
            
        }

        public class MyEventHandler : 
            IEventHandler<EntityChangedEvent<MyEntity>>,
            IEventHandler<EntityCreatedEvent<MyEntity>>
        {
            public int EntityChangedEventCount { get; set; }
            public int EntityCreatedEventCount { get; set; }

            public void Handle(EntityChangedEvent<MyEntity> @event)
            {
                EntityChangedEventCount++;
            }

            public void Handle(EntityCreatedEvent<MyEntity> @event)
            {
                EntityCreatedEventCount++;
            }
        }
    }
}
