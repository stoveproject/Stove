using System.Collections.Generic;

using Stove.Events;

using Xunit;

namespace Stove.Tests.Events.Bus
{
    public class TransientDisposableEventHandlerTest : EventBusTestBase
    {
        [Fact]
        public void Should_Call_Handler_AndDispose()
        {
            EventBus.Register<MySimpleEvent, MySimpleTransientEventHandler>();

            EventBus.Publish(new MySimpleEvent(1), new EventHeaders());
            EventBus.Publish(new MySimpleEvent(2), new EventHeaders());
            EventBus.Publish(new MySimpleEvent(3), new EventHeaders());

            Assert.Equal(MySimpleTransientEventHandler.HandleCount, 3);
            Assert.Equal(MySimpleTransientEventHandler.DisposeCount, 3);
        }
    }
}
