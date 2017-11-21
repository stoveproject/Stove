using Stove.NHibernate.EntityMappings;

namespace Stove.NHibernate.Tests.Entities
{
    public class CategoryMap : EntityMap<Category>
    {
        public CategoryMap() : base("Category")
        {
            this.MapFullAudited();

            Map(x => x.Name).Not.Nullable();
        }
    }
}
