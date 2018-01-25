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
                @event =>
                {
                    totalData += @event.Value;
                    //Assert.Equal(this, @event.EventSource);
                });

            EventBus.Publish(new MySimpleEvent(1)); //Should handle directly registered class
            EventBus.Publish(new MySimpleEvent(2)); //Should handle directly registered class
            EventBus.Publish(new MyDerivedEvent(3)); //Should handle derived class too
            EventBus.Publish(new MyDerivedEvent(4)); //Should handle derived class too

            Assert.Equal(10, totalData);
        }

        [Fact]
        public void Should_Not_Handle_Events_For_Base_Classes()
        {
            var totalData = 0;

            EventBus.Register<MyDerivedEvent>(
                @event =>
                {
                    totalData += @event.Value;
                    //Assert.Equal(this, @event.EventSource);
                });

            EventBus.Publish(new MySimpleEvent(1)); //Should not handle
            EventBus.Publish(new MySimpleEvent(2)); //Should not handle
            EventBus.Publish(new MyDerivedEvent(3)); //Should handle
            EventBus.Publish(new MyDerivedEvent(4)); //Should handle

            Assert.Equal(7, totalData);
        }   
    }
}