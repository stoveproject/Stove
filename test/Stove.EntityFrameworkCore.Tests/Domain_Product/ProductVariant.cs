using Stove.Domain.Entities;
using Stove.EntityFrameworkCore.Tests.Domain_Product.Events;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Stove.EntityFrameworkCore.Tests.Domain_Product
{
    public class ProductVariant : Entity<int>
    {
        [Required]
        public virtual Product Product { get; protected set; }

        public virtual string Barcode { get; protected set; }

        public virtual string Name { get; protected set; }

        public virtual ICollection<VariantValue> Values { get; protected set; }


        protected ProductVariant()
        {
            Register<VariantValueAdded>(When);
        }

        public ProductVariant(Product product, string barcode, string name) : this()
        {
            Product = product;
            Barcode = barcode;
            Name = name;
        }

        private void When(VariantValueAdded @event)
        {
            Values.Add(new VariantValue(this, @event.Value));
        }

    }
}
