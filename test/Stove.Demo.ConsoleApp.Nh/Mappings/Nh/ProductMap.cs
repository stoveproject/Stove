using Stove.Demo.ConsoleApp.Nh.Entities;
using Stove.NHibernate.EntityMappings;

namespace Stove.Demo.ConsoleApp.Nh.Mappings
{
    public class ProductMap : EntityMap<Product>
    {
        public ProductMap() : base("Products")
        {
            this.MapFullAudited();

            Map(x => x.Name);
        }
    }
}
