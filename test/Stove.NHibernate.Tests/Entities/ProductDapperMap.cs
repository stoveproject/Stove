using DapperExtensions.Mapper;

namespace Stove.NHibernate.Tests.Entities
{
    public sealed class ProductDapperMap : ClassMapper<Product>
    {
        public ProductDapperMap()
        {
            Table("Product");
	        Map(x => x.Id).Key(KeyType.Identity);
            AutoMap();
        }
    }
}
