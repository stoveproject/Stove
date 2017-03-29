using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Stove.Domain.Entities.Auditing;

namespace Stove.Dapper.Tests.Entities
{
    [Table("ProductDetails")]
    public class ProductDetail : FullAuditedEntity
    {
        protected ProductDetail()
        {
        }

        public ProductDetail(string gender) : this()
        {
            Gender = gender;
        }

        [Required]
        public string Gender { get; set; }
    }
}
