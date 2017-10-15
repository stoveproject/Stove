using Stove.Domain.Entities.Auditing;

namespace Stove.Couchbase.Tests.Entities
{
    public class Order : FullAuditedEntity<string>
    {
        protected Order()
        {
        }

        public Order(string address, Product product) : this()
        {
            Address = address;
            Product = product;
        }

        public string Address { get; set; }

        public Product Product { get; set; }
    }
}
