using System;
using System.ComponentModel.DataAnnotations.Schema;

using Stove.Domain.Entities;
using Stove.Domain.Entities.Auditing;

namespace Stove.Tests.SampleApplication.Domain.Entities
{
    [Table("User")]
    public class User : Entity<long>, IHasCreationTime
    {
        public string Name { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
