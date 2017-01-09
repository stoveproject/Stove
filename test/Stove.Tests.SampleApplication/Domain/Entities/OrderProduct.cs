using System.ComponentModel.DataAnnotations.Schema;

using Stove.Domain.Entities;

namespace Stove.Tests.SampleApplication.Domain.Entities
{
    [Table("OrderProduct")]
    public class OrderProduct : Entity
    {
        public Order Order { get; set; }

        public Product ProductId { get; set; }
    }
}
