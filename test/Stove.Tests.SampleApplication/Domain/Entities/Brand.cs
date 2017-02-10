using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using JetBrains.Annotations;

using Stove.Domain.Entities;

namespace Stove.Tests.SampleApplication.Domain.Entities
{
    [Table("Brand")]
    public class Brand : Entity
    {
        private Brand()
        {
        }

        [Required]
        [NotNull]
        public virtual string Name { get; protected set; }
    }
}
