using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using Stove.Tests.SampleApplication.Domain.Entities;

namespace Stove.Tests.SampleApplication.Domain.Mapping
{
    public class ProductBundleMap : EntityTypeConfiguration<ProductBundle>
    {
        public ProductBundleMap()
        {
            HasKey(x => x.Id);
           
            Property(x => x.Name);

            Property(ProductBundle.ProductIdsExpression);
        }
    }
}
