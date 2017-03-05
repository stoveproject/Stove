using System.IO;
using System.Text;

using Shouldly;

using Stove.IO.Extensions;

using Xunit;

namespace Stove.Tests.IO.Extensions
{
    public class StreamExtensions_Tests
    {
        [Fact]
        public void GetAllBytes_should_work()
        {
            byte[] bytes;
            using (var whatEverStream = new MemoryStream(Encoding.UTF8.GetBytes("whatever")))
            {
                bytes = whatEverStream.GetAllBytes();
            }

            bytes.ShouldNotBeNull();
        }
    }
}
