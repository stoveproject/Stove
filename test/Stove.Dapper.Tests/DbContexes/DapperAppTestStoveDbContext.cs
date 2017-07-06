using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Common;
using System.Data.SQLite;
using System.Data.SQLite.EF6;

using Stove.Dapper.Tests.Entities;
using Stove.EntityFramework;

namespace Stove.Dapper.Tests.DbContexes
{
    [DbConfigurationType(typeof(DapperDbContextConfiguration))]
    public class DapperAppTestStoveDbContext : StoveDbContext
    {
        public DapperAppTestStoveDbContext(DbConnection connection)
            : base(connection, false)
        {
        }

        public DapperAppTestStoveDbContext(DbConnection connection, bool contextOwnsConnection)
            : base(connection, contextOwnsConnection)
        {
        }

        public virtual IDbSet<Product> Products { get; set; }

        public virtual IDbSet<ProductDetail> ProductDetails { get; set; }
    }

    public class DapperDbContextConfiguration : DbConfiguration
    {
        public DapperDbContextConfiguration()
        {
            SetProviderFactory("System.Data.SQLite", SQLiteFactory.Instance);
            SetProviderServices("System.Data.SQLite", (DbProviderServices)SQLiteProviderFactory.Instance.GetService(typeof(DbProviderServices)));
        }
    }
}
