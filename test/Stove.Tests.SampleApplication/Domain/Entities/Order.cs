using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

using Stove.Domain.Entities.Auditing;

namespace Stove.Tests.SampleApplication.Domain.Entities
{
    [Table("Order")]
    public class Order : CreationAuditedEntity<int, User>
    {
        public virtual ICollection<OrderProduct> OrderProducts { get; protected set; }
    }
}
