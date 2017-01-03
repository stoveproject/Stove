using System.Data.Common;
using System.Data.Entity;

using Stove.Demo.Entities;
using Stove.EntityFramework.EntityFramework;

namespace Stove.Demo.DbContexes
{
    public class PersonStoveDbContext : StoveDbContext
    {
        public PersonStoveDbContext() : base("Default")
        {
        }

        public PersonStoveDbContext(DbConnection existingConnection) : base(existingConnection, false)
        {
        }

        public virtual IDbSet<Person> Persons { get; set; }
    }
}
