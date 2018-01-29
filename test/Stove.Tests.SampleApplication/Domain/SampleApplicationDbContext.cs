using System.Data.Common;
using System.Data.Entity;

using Stove.EntityFramework;
using Stove.Tests.SampleApplication.Domain.Entities;

namespace Stove.Tests.SampleApplication.Domain
{
    public class SampleApplicationDbContext : StoveDbContext
    {
        public SampleApplicationDbContext()
        {
        }

        public SampleApplicationDbContext(DbConnection connection)
            : base(connection, false)
        {
        }

        public virtual IDbSet<Product> Products { get; set; }

        public virtual IDbSet<Brand> Brands { get; set; }

        public virtual IDbSet<Category> Categories { get; set; }

        public virtual IDbSet<Gender> Genders { get; set; }

        public virtual IDbSet<ProductBrand> ProductBrands { get; set; }

        public virtual IDbSet<ProductCategory> ProductCategories { get; set; }

        public virtual IDbSet<ProductGender> ProductGenders { get; set; }

        public virtual IDbSet<User> Users { get; set; }

        public virtual IDbSet<ProductDetail> ProductDetails { get; set; }

        public virtual IDbSet<Message> Messages { get; set; }

        public virtual IDbSet<Campaign> Campaigns { get; set; }

        public virtual IDbSet<ProductBundle> ProductBundles { get; set; }
    }
}
