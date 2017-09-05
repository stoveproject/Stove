using Microsoft.EntityFrameworkCore;

namespace Stove.EntityFrameworkCore
{
	public interface IDbContextProvider<out TDbContext>
		where TDbContext : DbContext
	{
		TDbContext GetDbContext();
	}
}
