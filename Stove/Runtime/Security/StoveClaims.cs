namespace Stove.Runtime.Security
{
    /// <summary>
    /// Used to get Stove-specific claim type names.
    /// </summary>
    public static class StoveClaimTypes
    {
        /// <summary>
        /// TenantId.
        /// </summary>
        public const string TenantId = "http://www.aspnetboilerplate.com/identity/claims/tenantId";

        /// <summary>
        /// ImpersonatorUserId.
        /// </summary>
        public const string ImpersonatorUserId = "http://www.aspnetboilerplate.com/identity/claims/impersonatorUserId";
        
        /// <summary>
        /// ImpersonatorTenantId
        /// </summary>
        public const string ImpersonatorTenantId = "http://www.aspnetboilerplate.com/identity/claims/impersonatorTenantId";
    }
}
