using DapperExtensions.Mapper;

namespace Stove.Demo.WebApi.Core.Domain.Entities.Mappings.Dapper
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
