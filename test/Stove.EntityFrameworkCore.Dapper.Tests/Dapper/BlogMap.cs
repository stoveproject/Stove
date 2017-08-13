using DapperExtensions.Mapper;

using Stove.EntityFrameworkCore.Dapper.Tests.Domain;

namespace Stove.EntityFrameworkCore.Dapper.Tests.Dapper
{
    public sealed class BlogMap : ClassMapper<Blog>
    {
        public BlogMap()
        {
            Table("Blogs");
            Map(x => x.Id).Key(KeyType.Identity);
            Map(x => x.DomainEvents).Ignore();
            AutoMap();
        }
    }
}
