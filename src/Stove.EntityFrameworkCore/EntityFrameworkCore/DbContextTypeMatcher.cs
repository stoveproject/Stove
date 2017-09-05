using Stove.Domain.Uow;
using Stove.EntityFramework;

namespace Stove.EntityFrameworkCore
{
	public class DbContextTypeMatcher : DbContextTypeMatcher<StoveDbContext>
	{
		public DbContextTypeMatcher(ICurrentUnitOfWorkProvider currentUnitOfWorkProvider)
			: base(currentUnitOfWorkProvider)
		{
		}
	}
}
