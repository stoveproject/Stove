using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Stove.Domain.Entities;
using Stove.Domain.Entities.Auditing;

namespace Stove.Tests.SampleApplication.Domain.Entities
{
    [Table(nameof(User))]
    public class User : AggregateRoot, IHasCreationTime
    {
        [Required]
        public virtual string Name { get; set; }

        [Required]
        public virtual string Surname { get; set; }

        [Required]
        public virtual string Email { get; set; }

        [Required]
        public virtual DateTime CreationTime { get; set; }
    }
}
