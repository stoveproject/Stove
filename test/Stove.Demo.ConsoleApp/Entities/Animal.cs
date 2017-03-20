using System.ComponentModel.DataAnnotations.Schema;

using Stove.Domain.Entities;

namespace Stove.Demo.ConsoleApp.Entities
{
    [Table("Animals")]
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
