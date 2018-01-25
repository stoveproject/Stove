namespace Stove.Tests.Events.Bus
{
    public class MyDerivedEvent : MySimpleEvent
    {
        public MyDerivedEvent(int value)
            : base(value)
        {
        }
    }
}