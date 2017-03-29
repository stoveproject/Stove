using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Stove.Domain.Entities.Auditing;

namespace Stove.Dapper.Tests.Entities
{
    [Table("Products")]
    public class Product : FullAuditedEntity
    {
        protected Product()
        {
        }

        public Product(string name) : this()
        {
            Name = name;
        }

        [Required]
        public string Name { get; set; }
    }
}
