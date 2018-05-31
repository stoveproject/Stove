using Stove.Domain.Entities;
using Stove.EntityFrameworkCore.Tests.Domain_Product.Events;
using System;
using System.Collections.Generic;

namespace Stove.EntityFrameworkCore.Tests.Domain_Product
{
    public class Product : AggregateRoot
    {
        public virtual string Title { get; protected set; }

        public virtual string ProductCode { get; protected set; }

        public virtual ICollection<ProductVariant> Variants { get; protected set; }

        protected Product()
        {
            Register<VariantAdded>(When);
        }

        public Product(string title, string productCode) : this()
        {
            Title = title;
            ProductCode = productCode;
        }

        public void AddVariant(string barcode, string variantName)
        {
            if (barcode == null)
            {
                throw new ArgumentNullException(nameof(barcode));
            }

            if (variantName == null)
            {
                throw new ArgumentNullException(nameof(variantName));
            }

            ApplyChange(new VariantAdded(Id, barcode, variantName));
        }

        public void AddVariantValue(ProductVariant variant, string variantValue)
        {
            if (variantValue == null)
            {
                throw new ArgumentNullException(nameof(variantValue));
            }

            var @event = new VariantValueAdded(variant.Id, variantValue);
            variant.Route(@event);
            ApplyChange(@event);
        }
        
        private void When(VariantAdded @event)
        {
            var variant = new ProductVariant(this, @event.Barcode, @event.VariantName);

            Variants.Add(variant);
        }
          
    }
}
