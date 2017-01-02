using System.Data.Entity;

using Stove.Demo.Entities;
using Stove.EntityFramework.EntityFramework;

namespace Stove.Demo.DbContexes
{
    public class PersonStoveDbContext : StoveDbContext
    {
        public PersonStoveDbContext() : base("Person")
        {
        }

        public virtual IDbSet<Person> Persons { get; set; }
    }
}
