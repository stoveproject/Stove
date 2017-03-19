using System.Data.Common;
using System.Data.Entity;

using Stove.Dapper.Tests.Entities;
using Stove.EntityFramework;

namespace Stove.Dapper.Tests.DbContexes
{
    public class DapperAppTestStoveDbContext : StoveDbContext
    {
        public DapperAppTestStoveDbContext()
        {
        }

        public DapperAppTestStoveDbContext(DbConnection existingConnection, bool contextOwnsConnection) : base(existingConnection, contextOwnsConnection)
        {
        }

        public DapperAppTestStoveDbContext(DbConnection connection)
            : base(connection, false)
        {
        }

        public virtual IDbSet<User> Users { get; set; }
    }
}
