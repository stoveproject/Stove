using DapperExtensions.Mapper;

using Stove.Dapper.Tests.Entities;

namespace Stove.Dapper.Tests.Mappings
{
    public sealed class ProductDetailMap : ClassMapper<ProductDetail>
    {
        public ProductDetailMap()
        {
            Table("ProductDetails");
            Map(x => x.Id).Key(KeyType.Identity);
            AutoMap();
        }
    }
}
