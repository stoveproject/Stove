using System.Data.Common;
using System.Data.Entity;

using JetBrains.Annotations;

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

        [NotNull]
        public virtual IDbSet<Product> Products { get; set; }

        [NotNull]
        public virtual IDbSet<Brand> Brands { get; set; }

        [NotNull]
        public virtual IDbSet<Category> Categories { get; set; }

        [NotNull]
        public virtual IDbSet<Gender> Genders { get; set; }

        [NotNull]
        public virtual IDbSet<ProductBrand> ProductBrands { get; set; }

        [NotNull]
        public virtual IDbSet<ProductCategory> ProductCategories { get; set; }

        [NotNull]
        public virtual IDbSet<ProductGender> ProductGenders { get; set; }

        [NotNull]
        public virtual IDbSet<User> Users { get; set; }

        [NotNull]
        public virtual IDbSet<ProductDetail> ProductDetails { get; set; }
    }
}
