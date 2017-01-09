using System.ComponentModel.DataAnnotations.Schema;

using Stove.Domain.Entities;

namespace Stove.Demo.ConsoleApp.Entities
{
    [Table("Person")]
    public class Person : Entity
    {
        private Person()
        {
        }

        public Person(string name) : this()
        {
            Name = name;
        }

        public virtual string Name { get; set; }
    }
}
