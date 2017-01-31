using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;

using Autofac.Extras.IocManager;

using Stove.Runtime.Security;

namespace Stove.Runtime.Session
{
    /// <summary>
    ///     Implements <see cref="IStoveSession" /> to get session properties from claims of
    ///     <see cref="Thread.CurrentPrincipal" />.
    /// </summary>
    public class ClaimsStoveSession : StoveSessionBase, ISingletonDependency
    {
        public ClaimsStoveSession(
            IAmbientScopeProvider<SessionOverride> sessionOverrideScopeProvider,
            IPrincipalAccessor principalAccessor)
            : base(sessionOverrideScopeProvider)
        {
            PrincipalAccessor = principalAccessor;
        }

        protected IPrincipalAccessor PrincipalAccessor { get; }

        public override long? UserId
        {
            get
            {
                if (OverridedValue != null)
                {
                    return OverridedValue.UserId;
                }

                Claim userIdClaim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdClaim?.Value))
                {
                    return null;
                }

                long userId;
                if (!long.TryParse(userIdClaim.Value, out userId))
                {
                    return null;
                }

                return userId;
            }
        }

        public override long? ImpersonatorUserId
        {
            get
            {
                Claim impersonatorUserIdClaim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == StoveClaimTypes.ImpersonatorUserId);
                if (string.IsNullOrEmpty(impersonatorUserIdClaim?.Value))
                {
                    return null;
                }

                return Convert.ToInt64(impersonatorUserIdClaim.Value);
            }
        }
    }
}
