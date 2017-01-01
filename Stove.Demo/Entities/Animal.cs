using System.ComponentModel.DataAnnotations.Schema;

using Stove.Domain.Entities;

namespace Stove.Demo.Entities
{
    [Table("Animal")]
    public class Animal : Entity
    {
        private Animal()
        {
        }

        public Animal(string name) : this()
        {
            Name = name;
        }

        public virtual string Name { get; set; }
    }
}
