using System.Data.Entity;

namespace Stove.EntityFramework.EntityFramework.Uow
{
    public class ActiveDbContextInfo
    {
        public DbContext DbContext { get; }

        public ActiveDbContextInfo(DbContext dbContext)
        {
            DbContext = dbContext;
        }
    }
}