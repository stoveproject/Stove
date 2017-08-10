using System.Security.Claims;
using System.Threading;

using Autofac.Extras.IocManager;

namespace Stove.Runtime.Session
{
    public class DefaultPrincipalAccessor : IPrincipalAccessor, ISingletonDependency
    {
        public static DefaultPrincipalAccessor Instance => new DefaultPrincipalAccessor();

        public virtual ClaimsPrincipal Principal =>
#if NET461
            Thread.CurrentPrincipal as ClaimsPrincipal;
#else
		    null;
#endif
	}
}
