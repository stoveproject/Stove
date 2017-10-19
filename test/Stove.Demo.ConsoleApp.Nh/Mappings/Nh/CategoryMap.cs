using Stove.Demo.ConsoleApp.Nh.Entities;
using Stove.NHibernate.EntityMappings;

namespace Stove.Demo.ConsoleApp.Nh.Mappings.Nh
{
    public class CategoryMap : EntityMap<Category>
    {
        public CategoryMap() : base("Categories")
        {
            this.MapFullAudited();

            Map(x => x.Name);
        }
    }
}
