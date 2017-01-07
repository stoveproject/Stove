using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

using JetBrains.Annotations;

using Stove.Extensions;

namespace Stove.Runtime.Security
{
    public static class ClaimsIdentityExtensions
    {
        public static UserIdentifier GetUserIdentifierOrNull(this IIdentity identity)
        {
            Check.NotNull(identity, nameof(identity));

            long? userId = identity.GetUserId();
            if (userId == null)
            {
                return null;
            }

            return new UserIdentifier(userId.Value);
        }

        public static long? GetUserId([NotNull] this IIdentity identity)
        {
            Check.NotNull(identity, nameof(identity));

            var claimsIdentity = identity as ClaimsIdentity;

            Claim userIdOrNull = claimsIdentity?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdOrNull == null || userIdOrNull.Value.IsNullOrWhiteSpace())
            {
                return null;
            }

            return Convert.ToInt64(userIdOrNull.Value);
        }

        public static long? GetImpersonatorUserId(this IIdentity identity)
        {
            Check.NotNull(identity, nameof(identity));

            var claimsIdentity = identity as ClaimsIdentity;

            Claim userIdOrNull = claimsIdentity?.Claims?.FirstOrDefault(c => c.Type == StoveClaimTypes.ImpersonatorUserId);
            if (userIdOrNull == null || userIdOrNull.Value.IsNullOrWhiteSpace())
            {
                return null;
            }

            return Convert.ToInt64(userIdOrNull.Value);
        }
    }
}
