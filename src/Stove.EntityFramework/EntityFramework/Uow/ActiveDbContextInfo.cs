using System.Data.Entity;

namespace Stove.EntityFramework.EntityFramework.Uow
{
    public class ActiveDbContextInfo
    {
        public ActiveDbContextInfo(DbContext dbContext, string nameOrConnectionString)
        {
            DbContext = dbContext;
            NameOrConnectionString = nameOrConnectionString;
        }

        public DbContext DbContext { get; }

        public string NameOrConnectionString { get; }
    }
}
