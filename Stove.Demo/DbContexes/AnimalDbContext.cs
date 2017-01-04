using System.Data.Common;
using System.Data.Entity;

using Stove.Demo.Entities;
using Stove.EntityFramework.EntityFramework;

namespace Stove.Demo.DbContexes
{
    public class AnimalStoveDbContext : StoveDbContext
    {
        public AnimalStoveDbContext() : base("Default")
        {
        }

        public AnimalStoveDbContext(DbConnection existingConnection, bool contextOwnsConnection) : base(existingConnection, contextOwnsConnection)
        {
        }

        public virtual IDbSet<Animal> Animals { get; set; }
    }
}
