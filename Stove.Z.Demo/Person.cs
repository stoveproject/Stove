using System.ComponentModel.DataAnnotations.Schema;

using Stove.Domain.Entities;

namespace Stove.Demo
{
    [Table("Person")]
    public class Person : Entity
    {
        public virtual string Name { get; set; }
    }
}
