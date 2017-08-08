using Stove.Demo.ConsoleApp.Entities;
using Stove.Mapster;

namespace Stove.Demo.ConsoleApp.Dto
{
    [AutoMap(typeof(StorefrontProductPrice))]
    public class StorefrontProductPriceDto
    {
        public int ProductId { get; set; }

        public int StorefrontId { get; set; }

        public int CurrencyId { get; set; }

        public decimal Msrp { get; set; }

        public decimal SellingPrice { get; set; }
    }
}
