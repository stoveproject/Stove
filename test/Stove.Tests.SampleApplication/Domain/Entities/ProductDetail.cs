using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Stove.Domain.Entities;

namespace Stove.Tests.SampleApplication.Domain.Entities
{
    [Table(nameof(ProductDetail))]
    public class ProductDetail : Entity
    {
        [Required]
        public Product Product { get; set; }

        public virtual string Age { get; set; }
    }
}
