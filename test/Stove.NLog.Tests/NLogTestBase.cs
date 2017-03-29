using System.Reflection;

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
                builder.RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly()));
            });
        }
    }
}
