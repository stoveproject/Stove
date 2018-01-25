using Stove.Events.Bus;

namespace Stove.EntityFrameworkCore.Dapper.Tests.Domain
{
    public class BlogUrlChangedEvent : Event
    {
        public Blog Blog;
        public string Url;

        public BlogUrlChangedEvent(Blog blog, string url)
        {
            Blog = blog;
            Url = url;
        }
    }
}
