using System.Reflection;

using Stove.TestBase;

namespace Stove.Mapster.Tests
{
    public class StoveMapsterTestApplication : ApplicationTestBase<StoveMapsterTestBootstrapper>
    {
        public StoveMapsterTestApplication()
        {
            Building(builder => { builder.RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly())); });
        }
    }
}
