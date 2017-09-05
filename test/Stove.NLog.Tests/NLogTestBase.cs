using System.Reflection;

using Stove.Reflection.Extensions;
using Stove.TestBase;

namespace Stove.NLog.Tests
{
    public abstract class NLogTestBase : ApplicationTestBase<StoveNLogTestBootstrapper>
    {
        protected NLogTestBase()
        {
            Building(builder =>
            {
                builder.UseStoveNLog();
                builder.RegisterServices(r => r.RegisterAssemblyByConvention(typeof(NLogTestBase).GetAssembly()));
            });
        }
    }
}
