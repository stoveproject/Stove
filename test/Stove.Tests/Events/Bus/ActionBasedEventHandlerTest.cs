using System;

using Xunit;

namespace Stove.Tests.Events.Bus
{
    public class ActionBasedEventHandlerTest : EventBusTestBase
    {
        [Fact]
        public void Should_Call_Action_On_Event_With_Correct_Source()
        {
            var totalData = 0;

            EventBus.Register<MySimpleEventData>(
                eventData =>
                {
                    totalData += eventData.Value;
                    //Assert.Equal(this, this);
                });

            EventBus.Publish(new MySimpleEventData(1));
            EventBus.Publish(new MySimpleEventData(2));
            EventBus.Publish(new MySimpleEventData(3));
            EventBus.Publish(new MySimpleEventData(4));

            Assert.Equal(10, totalData);
        }

        [Fact]
        public void Should_Call_Handler_With_Non_Generic_Trigger()
        {
            var totalData = 0;

            EventBus.Register<MySimpleEventData>(
                eventData =>
                {
                    totalData += eventData.Value;
                    //Assert.Equal(this, this);
                });

            EventBus.Publish(typeof(MySimpleEventData), new MySimpleEventData(1));
            EventBus.Publish(typeof(MySimpleEventData), new MySimpleEventData(2));
            EventBus.Publish(typeof(MySimpleEventData), new MySimpleEventData(3));
            EventBus.Publish(typeof(MySimpleEventData), new MySimpleEventData(4));

            Assert.Equal(10, totalData);
        }

        [Fact]
        public void Should_Not_Call_Action_After_Unregister_1()
        {
            var totalData = 0;

            var registerDisposer = EventBus.Register<MySimpleEventData>(
                eventData =>
                {
                    totalData += eventData.Value;
                });

            EventBus.Publish(new MySimpleEventData(1));
            EventBus.Publish(new MySimpleEventData(2));
            EventBus.Publish(new MySimpleEventData(3));

            registerDisposer.Dispose();

            EventBus.Publish(new MySimpleEventData(4));

            Assert.Equal(6, totalData);
        }

        [Fact]
        public void Should_Not_Call_Action_After_Unregister_2()
        {
            var totalData = 0;

            var action = new Action<MySimpleEventData>(
                eventData =>
                {
                    totalData += eventData.Value;
                });

            EventBus.Register(action);

            EventBus.Publish(new MySimpleEventData(1));
            EventBus.Publish(new MySimpleEventData(2));
            EventBus.Publish(new MySimpleEventData(3));

            EventBus.Unregister(action);

            EventBus.Publish(new MySimpleEventData(4));

            Assert.Equal(6, totalData);
        }
    }
}
