using DapperExtensions.Mapper;

using Stove.EntityFrameworkCore.Dapper.Tests.Domain;

namespace Stove.EntityFrameworkCore.Dapper.Tests.Dapper
{
    public sealed class CommentMap : ClassMapper<Comment>
    {
        public CommentMap()
        {
            Table("Comments");
            Map(x => x.Id).Key(KeyType.Identity);
            AutoMap();
        }
    }
}
