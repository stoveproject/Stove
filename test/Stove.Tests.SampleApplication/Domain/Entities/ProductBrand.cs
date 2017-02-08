using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using JetBrains.Annotations;

using Stove.Domain.Entities;

namespace Stove.Tests.SampleApplication.Domain.Entities
{
    [Table("ProductBrand")]
    public class ProductBrand : Entity
    {
        private ProductBrand()
        {
        }

        [NotNull]
        [Required]
        public virtual Product Product { get; protected set; }
        public int ProductId { get; [UsedImplicitly] private set; }

        [NotNull]
        [Required]
        public virtual Brand Brand { get; protected set; }
        public int BrandId { get; [UsedImplicitly] private set; }
    }
}
