using System.Data.Entity;

namespace Stove.EntityFramework.Repositories
{
    public interface IRepositoryWithDbContext
    {
        DbContext GetDbContext();
    }
}