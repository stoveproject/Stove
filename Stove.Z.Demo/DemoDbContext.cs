using System.Data.Entity;

using Stove.EntityFramework.EntityFramework;

namespace Stove.Z.Demo
{
    public class DemoDbContext : StoveDbContext
    {
        public virtual IDbSet<Person> Persons { get; set; }
    }
}
