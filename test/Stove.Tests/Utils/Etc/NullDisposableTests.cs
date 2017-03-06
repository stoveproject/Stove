using Shouldly;

using Stove.Utils.Etc;

using Xunit;

namespace Stove.Tests.Utils.Etc
{
    public class NullDisposableTests
    {
        [Fact]
        public void should_be_instantiatable()
        {
            NullDisposable instance = NullDisposable.Instance;

            instance.ShouldNotBeNull();
        }

        [Fact]
        public void should_be_dispose()
        {
            using (NullDisposable.Instance)
            {
            }
        }
    }
}
