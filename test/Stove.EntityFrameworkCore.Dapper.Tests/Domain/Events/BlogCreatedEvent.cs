using Stove.Events.Bus;

namespace Stove.EntityFrameworkCore.Dapper.Tests.Domain.Events
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
