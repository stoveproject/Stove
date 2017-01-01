using System.Data.Entity;

using Stove.Demo.Entities;
using Stove.EntityFramework.EntityFramework;

namespace Stove.Demo.DbContexes
{
    public class DemoStoveDbContext : StoveDbContext
    {
        public DemoStoveDbContext() : base("Default")
        {
        }

        public virtual IDbSet<Person> Persons { get; set; }

        public virtual IDbSet<Animal> Animals { get; set; }
    }
}
