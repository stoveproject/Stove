using System.ComponentModel.DataAnnotations.Schema;

using Stove.Domain.Entities;

namespace Stove.Demo.ConsoleApp.Entities
{
    [Table("Persons")]
    public class Person : Entity, ISoftDelete
    {
        private Person()
        {
        }

        public Person(string name) : this()
        {
            Name = name;
        }

        public virtual string Name { get; set; }

        public bool IsDeleted { get; set; }
    }
}
