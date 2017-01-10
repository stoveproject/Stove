using System.ComponentModel.DataAnnotations.Schema;

using Stove.Domain.Entities;

namespace Stove.Tests.SampleApplication.Domain.Entities
{
    [Table(nameof(OrderDetail))]
    public class OrderDetail : Entity
    {
        public virtual Order Order { get; set; }

        public virtual Product Product { get; set; }

        public virtual int Quantity { get; set; }

        public virtual int Discount { get; set; }

        public virtual int UnitPrice { get; set; }
    }
}
