using Stove.NHibernate.EntityMappings;

namespace Stove.NHibernate.Tests.Entities
{
    public class ProductMap : EntityMap<Product>
    {
        public ProductMap() : base("Product")
        {
            this.MapFullAudited();

            Map(x => x.Name).Not.Nullable();
        }
    }
}
