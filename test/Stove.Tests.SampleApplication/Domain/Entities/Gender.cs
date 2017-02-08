using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Stove.Domain.Entities;

namespace Stove.Tests.SampleApplication.Domain.Entities
{
    [Table("Gender")]
    public class Gender : Entity
    {
        private Gender()
        {
        }

        [Required]
        public virtual string Name { get; protected set; }
    }
}
