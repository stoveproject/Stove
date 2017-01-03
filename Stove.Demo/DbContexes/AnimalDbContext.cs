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

        public AnimalStoveDbContext(DbConnection existingConnection) : base(existingConnection, true)
        {
        }

        public virtual IDbSet<Animal> Animals { get; set; }
    }
}
