using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Common;
using System.Data.SQLite;
using System.Data.SQLite.EF6;

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
            SetProviderFactory("System.Data.SQLite", SQLiteFactory.Instance);
            SetProviderServices("System.Data.SQLite", (DbProviderServices)SQLiteProviderFactory.Instance.GetService(typeof(DbProviderServices)));
        }
    }
}
