using Stove.Events.Bus;

namespace Stove.EntityFrameworkCore.Tests.Domain_Product.Events
{
    public class VariantAdded : Event
    {
        public int ProductId { get; }

        public string Barcode { get; set; }

        public string VariantName { get; set; }

        public VariantAdded(int productId, string barcode, string variantName)
        {
            ProductId = productId;
            Barcode = barcode;
            VariantName = variantName;
        }
    }
}
