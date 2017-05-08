using System.Data.Common;
using System.Data.Entity;

using Stove.Demo.WebApi.Entities;
using Stove.EntityFramework;

namespace Stove.Demo.WebApi.DbContexts
{
    public class StoveDemoDbContext : StoveDbContext
    {
        public StoveDemoDbContext() : base("Default")
        {
        }

        public StoveDemoDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        protected StoveDemoDbContext(DbConnection existingConnection, bool contextOwnsConnection) : base(existingConnection, contextOwnsConnection)
        {
        }

        public virtual IDbSet<Product> Products { get; set; }
    }
}
