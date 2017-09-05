using DapperExtensions.Mapper;

using Stove.EntityFrameworkCore.Dapper.Tests.Domain;

namespace Stove.EntityFrameworkCore.Dapper.Tests.Dapper
{
    public class PostMap : ClassMapper<Post>
    {
        public PostMap()
        {
            Table("Posts");
            Map(x => x.Id).Key(KeyType.Guid);
            Map(x => x.Blog).Ignore();
            Map(x => x.Comment).Ignore();
            AutoMap();
        }
    }
}
