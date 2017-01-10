using System;
using System.ComponentModel.DataAnnotations.Schema;

using Stove.Domain.Entities;
using Stove.Domain.Entities.Auditing;

namespace Stove.Tests.SampleApplication.Domain.Entities
{
    [Table(nameof(Product))]
    public class Product : AggregateRoot, ICreationAudited
    {
        public virtual string Name { get; set; }

        public virtual string Code { get; set; }

        public virtual string Description { get; set; }

        public ProductDetail ProductDetail { get; set; }

        public virtual DateTime CreationTime { get; set; }

        public virtual long? CreatorUserId { get; set; }
    }
}
