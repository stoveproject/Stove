using Stove.Events.Bus;

namespace Stove.Tests.Events.Bus
{
    public class MySimpleEvent : Event
    {
        public int Value { get; set; }

        public MySimpleEvent(int value)
        {
            Value = value;
        }
    }
}