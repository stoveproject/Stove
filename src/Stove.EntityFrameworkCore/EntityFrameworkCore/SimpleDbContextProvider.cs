using Microsoft.EntityFrameworkCore;

namespace Stove.EntityFrameworkCore
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
