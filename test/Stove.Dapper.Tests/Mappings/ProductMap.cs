using DapperExtensions.Mapper;

using Stove.Dapper.Tests.Entities;

namespace Stove.Dapper.Tests.Mappings
{
    public sealed class ProductMap : ClassMapper<Product>
    {
        public ProductMap()
        {
            Table("Products");
            Map(x => x.Id).Key(KeyType.Identity);
            AutoMap();
        }
    }
}
