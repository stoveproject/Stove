using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Stove.Domain.Entities;

namespace Stove.Tests.SampleApplication.Domain.Entities
{
    [Table("ProductCategory")]
    public class ProductCategory : Entity
    {
        private ProductCategory()
        {
        }

        [Required]
        public virtual Category Category { get; protected set; }

        [Required]
        public virtual Product Product { get; protected set; }
    }
}
