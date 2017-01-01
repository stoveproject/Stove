using System.Data.Entity;

using Stove.EntityFramework.EntityFramework;

namespace Stove.Demo
{
    public class DemoStoveDbContext : StoveDbContext
    {
        public DemoStoveDbContext() : base("Default")
        {
        }

        public virtual IDbSet<Person> Persons { get; set; }
    }
}
