using System.Reflection;

using Stove.Reflection.Extensions;
using Stove.TestBase;

namespace Stove.Mapster.Tests
{
    public class StoveMapsterTestApplication : ApplicationTestBase<StoveMapsterTestBootstrapper>
    {
        public StoveMapsterTestApplication()
        {
            Building(builder =>
            {
                builder
                    .UseStoveMapster()
                    .RegisterServices(r => r.RegisterAssemblyByConvention(typeof(StoveMapsterTestApplication).GetAssembly()));
            }).Ok();
        }
    }
}
