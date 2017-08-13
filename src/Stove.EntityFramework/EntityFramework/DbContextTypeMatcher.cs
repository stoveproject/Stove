using Stove.Domain.Uow;
using Stove.EntityFramework.Common;

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
