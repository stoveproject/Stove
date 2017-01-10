using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

using Stove.Domain.Entities.Auditing;

namespace Stove.Tests.SampleApplication.Domain.Entities
{
    [Table(nameof(Cart))]
    public class Cart : CreationAuditedAggregateRoot<long, User>
    {
        public virtual ICollection<Product> Products { get; set; }
    }
}
