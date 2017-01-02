using System.Data.Entity;

using Stove.Demo.Entities;
using Stove.EntityFramework.EntityFramework;

namespace Stove.Demo.DbContexes
{
    public class AnimalStoveDbContext : StoveDbContext
    {
        public virtual IDbSet<Animal> Animals { get; set; }
    }
}
