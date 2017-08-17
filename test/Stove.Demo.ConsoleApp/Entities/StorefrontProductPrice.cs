using System;

using Stove.Domain.Entities;

namespace Stove.Demo.ConsoleApp.Entities
{
    public class StorefrontProductPrice : Entity
    {
        protected StorefrontProductPrice()
        {
        }

        public StorefrontProductPrice(int productId, int storefrontId, int currencyId, decimal msrp) : this()
        {
            ProductId = productId;
            StorefrontId = storefrontId;
            CurrencyId = currencyId;
            Msrp = msrp;
        }

        public virtual int ProductId { get; protected set; }

        public virtual int StorefrontId { get; protected set; }

        public virtual int CurrencyId { get; protected set; }

        public virtual decimal Msrp { get; protected set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
