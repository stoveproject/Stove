using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Stove.Domain.Entities;

namespace Stove.Tests.SampleApplication.Domain.Entities
{
    [Table("Category")]
    public class Category : Entity
    {
        private Category()
        {
        }

        [Required]
        public virtual string Name { get; protected set; }
    }
}
