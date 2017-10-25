using System.ComponentModel.DataAnnotations;

using Couchbase.Linq.Filters;

using Newtonsoft.Json;

using Stove.Domain.Entities;

namespace Stove.Couchbase.Tests.Entities
{
    [DocumentTypeFilter("product")]
    public class Product : Entity<string>
    {
        protected Product()
        {
        }

        public Product(string name) : this()
        {
            Name = name;
        }

        [Key]
        [JsonProperty("id")]
        public override string Id { get; set; }

        [JsonProperty("name")]
        public virtual string Name { get; set; }

        [JsonProperty("type")]
        public virtual string Type => "product";
    }
}
