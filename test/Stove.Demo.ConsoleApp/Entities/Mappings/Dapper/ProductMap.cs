using DapperExtensions.Mapper;

namespace Stove.Demo.ConsoleApp.Entities.Mappings.Dapper
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
