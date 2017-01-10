using System;
using System.ComponentModel.DataAnnotations.Schema;

using Stove.Domain.Entities;
using Stove.Domain.Entities.Auditing;

namespace Stove.Tests.SampleApplication.Domain.Entities
{
    [Table(nameof(User))]
    public class User : Entity<long>, IHasCreationTime
    {
        public virtual string Name { get; set; }

        public virtual string Surname { get; set; }

        public virtual string Email { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
