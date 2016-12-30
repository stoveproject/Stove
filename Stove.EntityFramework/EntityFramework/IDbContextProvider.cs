using System.Data.Entity;

namespace Stove.EntityFramework.EntityFramework
{
    /// <summary>
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    public interface IDbContextProvider<out TDbContext>
        where TDbContext : DbContext
    {
        TDbContext GetDbContext();
    }
}
