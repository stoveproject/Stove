using Autofac.Extras.IocManager;

using Stove.Runtime.Session;

namespace Stove.TestBase
{
    public class TestStoveSession : IStoveSession, ISingletonDependency
    {
        public long? UserId { get; set; }

        public long? ImpersonatorUserId { get; set; }
    }
}
