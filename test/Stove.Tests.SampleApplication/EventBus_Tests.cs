using Autofac.Extras.IocManager;

using Stove.Events.Bus;
using Stove.Events.Bus.Handlers;

using Xunit;

namespace Stove.Tests.SampleApplication
{
    public class EventBus_Tests : SampleApplicationTestBase
    {
        public EventBus_Tests()
        {
            Building(builder => { }).Ok();
        }

        [Fact]
        public void one_eventdata_with_multiple_handler()
        {
            The<IEventBus>().Trigger(new SomeEvent
            {
                ExecutionCount = 0
            });

            The<IEventBus>().Trigger(new SomeEvent2
            {
                ExecutionCount = 0
            });
        }

        public class SomeEvent : EventData
        {
            public int ExecutionCount { get; set; }
        }

        public class SomeEvent2 : EventData
        {
            public int ExecutionCount { get; set; }
        }

        public class SomeEventHandler : IEventHandler<SomeEvent>, IEventHandler<SomeEvent2>, ITransientDependency
        {
            public void HandleEvent(SomeEvent eventData)
            {
                eventData.ExecutionCount++;
            }

            public void HandleEvent(SomeEvent2 eventData)
            {
                eventData.ExecutionCount++;
            }
        }
    }
}
