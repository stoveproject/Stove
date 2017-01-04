using Shouldly;

using Stove.Json;

using Xunit;

namespace Stove.Tests.Json
{
    public class JsonExtensions_Tests
    {
        [Fact]
        public void ToJsonString_Test()
        {
            42.ToJsonString().ShouldBe("42");
        }
    }
}
