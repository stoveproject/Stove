using DapperExtensions.Mapper;

using Stove.Demo.ConsoleApp.Nh.Entities;

namespace Stove.Demo.ConsoleApp.Nh.Mappings.Dapper
{
    public sealed class CategoryMap : ClassMapper<Category>
    {
        public CategoryMap()
        {
            Table("Products");
            Map(x => x.Id).Key(KeyType.Identity);
            AutoMap();
        }
    }
}
