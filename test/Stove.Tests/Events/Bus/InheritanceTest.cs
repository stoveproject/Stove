using System.Collections.Generic;

using Xunit;

namespace Stove.Tests.Events.Bus
{
    public class InheritanceTest : EventBusTestBase
    {
        [Fact]
        public void Should_Handle_Events_For_Derived_Classes()
        {
            var totalData = 0;

            EventBus.Register<MySimpleEvent>(
                (@event, headers) => 
                {
                    totalData += @event.Value;
                    //Assert.Equal(this, @event.EventSource);
                });

            EventBus.Publish(new MySimpleEvent(1), new Dictionary<string, object>()); //Should handle directly registered class
            EventBus.Publish(new MySimpleEvent(2), new Dictionary<string, object>()); //Should handle directly registered class
            EventBus.Publish(new MyDerivedEvent(3), new Dictionary<string, object>()); //Should handle derived class too
            EventBus.Publish(new MyDerivedEvent(4), new Dictionary<string, object>()); //Should handle derived class too

            Assert.Equal(10, totalData);
        }

        [Fact]
        public void Should_Not_Handle_Events_For_Base_Classes()
        {
            var totalData = 0;

            EventBus.Register<MyDerivedEvent>(
                (@event, headers) => 
                {
                    totalData += @event.Value;
                    //Assert.Equal(this, @event.EventSource);
                });

            EventBus.Publish(new MySimpleEvent(1), new Dictionary<string, object>()); //Should not handle
            EventBus.Publish(new MySimpleEvent(2), new Dictionary<string, object>()); //Should not handle
            EventBus.Publish(new MyDerivedEvent(3), new Dictionary<string, object>()); //Should handle
            EventBus.Publish(new MyDerivedEvent(4), new Dictionary<string, object>()); //Should handle

            Assert.Equal(7, totalData);
        }   
    }
}