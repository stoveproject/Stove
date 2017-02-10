using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using JetBrains.Annotations;

using Stove.Domain.Entities;

namespace Stove.Tests.SampleApplication.Domain.Entities
{
    [Table("ProductGender")]
    public class ProductGender : Entity
    {
        private ProductGender()
        {
        }

        [Required]
        [NotNull]
        public virtual Gender Gender { get; protected set; }
        public int GenderId { get; [UsedImplicitly] private set; }

        [Required]
        [NotNull]
        public virtual Product Product { get; protected set; }
        public int ProductId { get; [UsedImplicitly] private set; }
    }
}
