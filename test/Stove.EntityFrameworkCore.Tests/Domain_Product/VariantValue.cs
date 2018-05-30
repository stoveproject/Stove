using Stove.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Stove.EntityFrameworkCore.Tests.Domain_Product
{
    public class VariantValue : Entity<int>
    {
        [Required]
        public virtual ProductVariant Variant { get; protected set; }

        public virtual string Value { get; protected set; }

        protected VariantValue() { }

        public VariantValue(ProductVariant variant, string value)
        {
            Variant = variant;
            Value = value;
        }
    }
}
