using Stove.Events.Bus;

namespace Stove.EntityFrameworkCore.Tests.Domain.Events
{
    public class BlogCreatedEvent : Event
    {
        public readonly string Name;

        public BlogCreatedEvent(string name)
        {
            Name = name;
        }
    }
}
