using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.SqlServer;

using Stove.Dapper.Tests.Entities;
using Stove.EntityFramework;

namespace Stove.Dapper.Tests.DbContexes
{
    [DbConfigurationType(typeof(MailDbContextConfiguration))]
    public class MailDbContext : StoveDbContext
    {
        public MailDbContext(DbConnection connection)
            : base(connection, false)
        {
        }

        public MailDbContext(DbConnection connection, bool contextOwnsConnection)
            : base(connection, contextOwnsConnection)
        {
        }

        public virtual IDbSet<Mail> Mails { get; set; }
    }

    public class MailDbContextConfiguration : DbConfiguration
    {
        public MailDbContextConfiguration()
        {
            SetProviderServices(
                "System.Data.SqlClient",
                SqlProviderServices.Instance
            );
        }
    }
}
