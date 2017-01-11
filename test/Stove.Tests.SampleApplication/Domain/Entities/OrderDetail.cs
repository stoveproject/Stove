using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Stove.Domain.Entities;

namespace Stove.Tests.SampleApplication.Domain.Entities
{
    [Table(nameof(OrderDetail))]
    public class OrderDetail : Entity
    {
        [Required]
        public virtual Order Order { get; set; }

        [Required]
        public virtual Product Product { get; set; }

        [Required]
        public virtual int Quantity { get; set; }

        [Required]
        public virtual int Discount { get; set; }

        [Required]
        public virtual int UnitPrice { get; set; }
    }
}
