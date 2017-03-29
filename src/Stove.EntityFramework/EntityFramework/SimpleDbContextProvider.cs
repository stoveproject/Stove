using System.Data.Entity;

namespace Stove.EntityFramework
{
    public sealed class SimpleDbContextProvider<TDbContext> : IDbContextProvider<TDbContext>
        where TDbContext : DbContext
    {
        public SimpleDbContextProvider(TDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public TDbContext DbContext { get; }

        public TDbContext GetDbContext()
        {
            return DbContext;
        }
    }
}
