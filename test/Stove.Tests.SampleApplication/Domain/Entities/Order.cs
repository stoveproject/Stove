using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

using Stove.Domain.Entities;
using Stove.Domain.Entities.Auditing;

namespace Stove.Tests.SampleApplication.Domain.Entities
{
    [Table(nameof(Order))]
    public class Order : AggregateRoot, ICreationAudited
    {
        public virtual ICollection<OrderDetail> OrderProducts { get; set; }

        public DateTime CreationTime { get; set; }

        public long? CreatorUserId { get; set; }
    }
}
