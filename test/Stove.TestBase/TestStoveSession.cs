using System;

using Autofac.Extras.IocManager;

using Stove.Runtime;
using Stove.Runtime.Session;

namespace Stove.TestBase
{
    public class TestStoveSession : IStoveSession, ISingletonDependency
    {
        private readonly IAmbientScopeProvider<SessionOverride> _sessionOverrideScopeProvider;

        private long? _userId;

        public TestStoveSession(IAmbientScopeProvider<SessionOverride> sessionOverrideScopeProvider)
        {
            _sessionOverrideScopeProvider = sessionOverrideScopeProvider;
        }

        public long? UserId
        {
            get
            {
                if (_sessionOverrideScopeProvider.GetValue(StoveSessionBase.SessionOverrideContextKey) != null)
                {
                    return _sessionOverrideScopeProvider.GetValue(StoveSessionBase.SessionOverrideContextKey).UserId;
                }

                return _userId;
            }
            set { _userId = value; }
        }

        public long? ImpersonatorUserId { get; set; }

        public IDisposable Use(long? userId)
        {
            return _sessionOverrideScopeProvider.BeginScope(StoveSessionBase.SessionOverrideContextKey, new SessionOverride(userId));
        }
    }
}
