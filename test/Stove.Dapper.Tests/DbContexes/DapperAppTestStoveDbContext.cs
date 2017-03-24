using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.SqlServer;

using Stove.Dapper.Tests.Entities;
using Stove.EntityFramework;

namespace Stove.Dapper.Tests.DbContexes
{
    [DbConfigurationType(typeof(DapperDbContextConfiguration))]
    public class SampleDapperApplicationDbContext : StoveDbContext
    {
        public SampleDapperApplicationDbContext(DbConnection connection)
            : base(connection, false)
        {
        }

        public SampleDapperApplicationDbContext(DbConnection connection, bool contextOwnsConnection)
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
            SetProviderServices(
                "System.Data.SqlClient",
                SqlProviderServices.Instance
            );
        }
    }
}
