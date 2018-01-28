using System;
using System.Collections.Generic;

using Stove.Events;

using Xunit;

namespace Stove.Tests.Events.Bus
{
    public class ActionBasedEventHandlerTest : EventBusTestBase
    {
        [Fact]
        public void Should_Call_Action_On_Event_With_Correct_Source()
        {
            var totalData = 0;

            EventBus.Register<MySimpleEvent>(
                (@event, headers) =>
                {
                    totalData += @event.Value;

                    //Assert.Equal(this, this);
                });

            EventBus.Publish(new MySimpleEvent(1), new EventHeaders());
            EventBus.Publish(new MySimpleEvent(2), new EventHeaders());
            EventBus.Publish(new MySimpleEvent(3), new EventHeaders());
            EventBus.Publish(new MySimpleEvent(4), new EventHeaders());

            Assert.Equal(10, totalData);
        }

        [Fact]
        public void Should_Call_Handler_With_Non_Generic_Trigger()
        {
            var totalData = 0;

            EventBus.Register<MySimpleEvent>(
                (@event, headers) =>
                {
                    totalData += @event.Value;

                    //Assert.Equal(this, this);
                });

            EventBus.Publish(typeof(MySimpleEvent), new MySimpleEvent(1), new EventHeaders());
            EventBus.Publish(typeof(MySimpleEvent), new MySimpleEvent(2), new EventHeaders());
            EventBus.Publish(typeof(MySimpleEvent), new MySimpleEvent(3), new EventHeaders());
            EventBus.Publish(typeof(MySimpleEvent), new MySimpleEvent(4), new EventHeaders());

            Assert.Equal(10, totalData);
        }

        [Fact]
        public void Should_Not_Call_Action_After_Unregister_1()
        {
            var totalData = 0;

            var registerDisposer = EventBus.Register<MySimpleEvent>(
                (@event, headers) =>
                {
                    totalData += @event.Value;
                });

            EventBus.Publish(new MySimpleEvent(1), new EventHeaders());
            EventBus.Publish(new MySimpleEvent(2), new EventHeaders());
            EventBus.Publish(new MySimpleEvent(3), new EventHeaders());

            registerDisposer.Dispose();

            EventBus.Publish(new MySimpleEvent(4), new EventHeaders());

            Assert.Equal(6, totalData);
        }

        [Fact]
        public void Should_Not_Call_Action_After_Unregister_2()
        {
            var totalData = 0;

            var action = new Action<MySimpleEvent, EventHeaders>(
                (@event, headers) =>
                {
                    totalData += @event.Value;
                });

            EventBus.Register(action);

            EventBus.Publish(new MySimpleEvent(1), new EventHeaders());
            EventBus.Publish(new MySimpleEvent(2), new EventHeaders());
            EventBus.Publish(new MySimpleEvent(3), new EventHeaders());

            EventBus.Unregister(action);

            EventBus.Publish(new MySimpleEvent(4), new EventHeaders());

            Assert.Equal(6, totalData);
        }
    }
}
