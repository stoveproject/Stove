using DapperExtensions.Mapper;

using Stove.Demo.ConsoleApp.Nh.Entities;

namespace Stove.Demo.ConsoleApp.Nh.Mappings.Dapper
{
    public sealed class ProductMap : ClassMapper<Product>
    {
        public ProductMap()
        {
            Table("Products");
            AutoMap();
        }
    }
}
