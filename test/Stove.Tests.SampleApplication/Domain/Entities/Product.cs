using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Stove.Domain.Entities;
using Stove.Domain.Entities.Auditing;

namespace Stove.Tests.SampleApplication.Domain.Entities
{
    [Table(nameof(Product))]
    public class Product : AggregateRoot, ICreationAudited
    {
        [Required]
        public virtual string Name { get; set; }

        [Required]
        public virtual string Code { get; set; }

        [Required]
        public virtual string Description { get; set; }

        public ProductDetail ProductDetail { get; set; }

        [Required]
        public virtual DateTime CreationTime { get; set; }

        public virtual long? CreatorUserId { get; set; }
    }
}
