using System.Data.Entity;

namespace Stove.EntityFramework.EntityFramework.Repositories
{
    public interface IRepositoryWithDbContext
    {
        DbContext GetDbContext();
    }
}