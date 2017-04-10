using Stove.Domain.Entities.Auditing;

namespace Stove.RavenDB.Tests.Entities
{
    public class Order : FullAuditedEntity
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
