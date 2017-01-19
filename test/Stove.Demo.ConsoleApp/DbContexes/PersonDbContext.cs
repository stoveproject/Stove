using System.Data.Common;
using System.Data.Entity;

using Stove.Demo.ConsoleApp.Entities;
using Stove.EntityFramework.EntityFramework;

namespace Stove.Demo.ConsoleApp.DbContexes
{
    public class PersonStoveDbContext : StoveDbContext
    {
        public PersonStoveDbContext() : base("Default")
        {
        }

        public PersonStoveDbContext(DbConnection existingConnection, bool contextOwnsConnection) : base(existingConnection, contextOwnsConnection)
        {
        }

        protected PersonStoveDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        public virtual IDbSet<Person> Persons { get; set; }
    }
}
