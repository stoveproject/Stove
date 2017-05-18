using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Stove.Domain.Entities;
using Stove.Domain.Entities.Auditing;
using Stove.Mapster;
using Stove.Tests.SampleApplication.Dtos;

namespace Stove.Tests.SampleApplication.Domain.Entities
{
    [Table(nameof(User))]
    [AutoMapFrom(typeof(UserDto))]
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
