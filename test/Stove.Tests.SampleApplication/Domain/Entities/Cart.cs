using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Stove.Domain.Entities;
using Stove.Domain.Entities.Auditing;

namespace Stove.Tests.SampleApplication.Domain.Entities
{
    [Table(nameof(Cart))]
    public class Cart : AggregateRoot, ICreationAudited
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public DateTime CreationTime { get; set; }

        public long? CreatorUserId { get; set; }
    }
}
