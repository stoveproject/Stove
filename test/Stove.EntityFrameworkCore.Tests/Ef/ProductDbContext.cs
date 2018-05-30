using Microsoft.EntityFrameworkCore;
using Stove.EntityFrameworkCore.Tests.Domain_Product;

namespace Stove.EntityFrameworkCore.Tests.Ef
{
    public class ProductDbContext : StoveDbContext
    {
        public DbSet<Product> Products { get; set; }
         
        public DbSet<ProductVariant> ProductVariants { get; set; }

        public DbSet<VariantValue> VaraintValues { get; set; }

        public ProductDbContext(DbContextOptions<ProductDbContext> options)
            : base(options)
        {
        }
    }
}
