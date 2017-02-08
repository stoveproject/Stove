using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using JetBrains.Annotations;

using Stove.Domain.Entities;

namespace Stove.Tests.SampleApplication.Domain.Entities
{
    [Table("Product")]
    public class Product : Entity
    {
        private Product()
        {
            _productCategories = new List<ProductCategory>();
        }

        [Required]
        [NotNull]
        public virtual string Name { get; protected set; }

        [NotNull]
        [InverseProperty("Product")]
        public virtual ProductBrand ProductBrand { get; protected set; }

        [NotNull]
        [InverseProperty("Product")]
        public virtual ProductGender ProductGender { get; protected set; }

        [NotNull]
        [InverseProperty("Product")]
        public virtual ProductDetail ProductDetail { get; protected set; }

        [NotNull]
        [ForeignKey("ProductId")]
        public IReadOnlyCollection<ProductCategory> ProductCategories => _productCategories.ToImmutableList();

        [NotNull]
        protected virtual ICollection<ProductCategory> _productCategories { get; set; }
    }
}
