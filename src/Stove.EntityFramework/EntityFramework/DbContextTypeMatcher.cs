using Stove.Domain.Uow;

namespace Stove.EntityFramework
{
	public class DbContextTypeMatcher : DbContextTypeMatcher<StoveDbContext>
	{
		public DbContextTypeMatcher(ICurrentUnitOfWorkProvider currentUnitOfWorkProvider)
			: base(currentUnitOfWorkProvider)
		{
		}
	}
}
