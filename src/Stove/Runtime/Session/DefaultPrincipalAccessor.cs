using System.Security.Claims;
using System.Threading;

using Autofac.Extras.IocManager;

namespace Stove.Runtime.Session
{
	public class DefaultPrincipalAccessor : IPrincipalAccessor, ITransientDependency
	{
		public static DefaultPrincipalAccessor Instance => new DefaultPrincipalAccessor();

		public virtual ClaimsPrincipal Principal => Thread.CurrentPrincipal as ClaimsPrincipal;
	}
}
