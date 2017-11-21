using System.ComponentModel.DataAnnotations;

using Couchbase.Linq.Filters;

using Newtonsoft.Json;

using Stove.Domain.Entities.Auditing;

namespace Stove.Couchbase.Tests.Entities
{
    [DocumentTypeFilter("order")]
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

        [Key]
        [JsonProperty("id")]
        public override string Id { get; set; }

        [JsonProperty("address")]
        public virtual string Address { get; set; }

        [JsonProperty("product")]
        public virtual Product Product { get; set; }

        [JsonProperty("type")]
        public virtual string Type => "product";
    }
}
