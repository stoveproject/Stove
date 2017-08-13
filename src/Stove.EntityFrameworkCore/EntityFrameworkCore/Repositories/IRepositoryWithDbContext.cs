using Microsoft.EntityFrameworkCore;

namespace Stove.EntityFrameworkCore.Repositories
{
    public interface IRepositoryWithDbContext
    {
        DbContext GetDbContext();
    }
}