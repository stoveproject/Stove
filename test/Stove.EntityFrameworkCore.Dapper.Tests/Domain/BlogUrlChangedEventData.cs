using Stove.Events.Bus;

namespace Stove.EntityFrameworkCore.Dapper.Tests.Domain
{
    public class BlogUrlChangedEventData : EventData
    {
        public Blog Blog;
        public string Url;

        public BlogUrlChangedEventData(Blog blog, string url)
        {
            Blog = blog;
            Url = url;
        }
    }
}
