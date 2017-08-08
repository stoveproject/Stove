using System.Data.Common;
using System.Data.Entity;

using Stove.Demo.ConsoleApp.Entities;
using Stove.EntityFramework;

namespace Stove.Demo.ConsoleApp.DbContexes
{
    public class PriceDbContext : StoveDbContext
    {
        public PriceDbContext() : base("Price")
        {
        }

        public PriceDbContext(DbConnection existingConnection, bool contextOwnsConnection) : base(existingConnection, contextOwnsConnection)
        {
        }

        protected PriceDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        public virtual IDbSet<StorefrontProductPrice> StorefrontProductPrice { get; set; }
    }
}
