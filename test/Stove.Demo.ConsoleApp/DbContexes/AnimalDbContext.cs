using System.Data.Common;
using System.Data.Entity;

using Stove.Demo.ConsoleApp.Entities;
using Stove.EntityFramework.EntityFramework;

namespace Stove.Demo.ConsoleApp.DbContexes
{
    public class AnimalStoveDbContext : StoveDbContext
    {
        public AnimalStoveDbContext() : base("Default")
        {
        }

        public AnimalStoveDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        public AnimalStoveDbContext(DbConnection existingConnection, bool contextOwnsConnection) : base(existingConnection, contextOwnsConnection)
        {
        }

        public virtual IDbSet<Animal> Animals { get; set; }
    }
}
