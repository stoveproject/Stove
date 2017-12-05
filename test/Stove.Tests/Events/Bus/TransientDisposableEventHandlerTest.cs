using Xunit;

namespace Stove.Tests.Events.Bus
{
    public class TransientDisposableEventHandlerTest : EventBusTestBase
    {
        [Fact]
        public void Should_Call_Handler_AndDispose()
        {
            EventBus.Register<MySimpleEventData, MySimpleTransientEventHandler>();

            EventBus.Publish(new MySimpleEventData(1));
            EventBus.Publish(new MySimpleEventData(2));
            EventBus.Publish(new MySimpleEventData(3));

            Assert.Equal(MySimpleTransientEventHandler.HandleCount, 3);
            Assert.Equal(MySimpleTransientEventHandler.DisposeCount, 3);
        }
    }
}
