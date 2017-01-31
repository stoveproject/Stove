using System;

namespace Stove.Runtime.Session
{
    public abstract class StoveSessionBase : IStoveSession
    {
        public const string SessionOverrideContextKey = "Stove.Runtime.Session.Override";

        protected StoveSessionBase(IAmbientScopeProvider<SessionOverride> sessionOverrideScopeProvider)
        {
            SessionOverrideScopeProvider = sessionOverrideScopeProvider;
        }

        protected SessionOverride OverridedValue => SessionOverrideScopeProvider.GetValue(SessionOverrideContextKey);

        protected IAmbientScopeProvider<SessionOverride> SessionOverrideScopeProvider { get; }

        public abstract long? UserId { get; }

        public abstract long? ImpersonatorUserId { get; }

        public IDisposable Use(long? userId)
        {
            return SessionOverrideScopeProvider.BeginScope(SessionOverrideContextKey, new SessionOverride(userId));
        }
    }
}
