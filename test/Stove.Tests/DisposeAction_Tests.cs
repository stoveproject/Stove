using Chill;

using Shouldly;

using Xunit;

namespace Stove.Tests
{
    public class DisposeAction_Test : GivenWhenThen
    {
        public DisposeAction_Test()
        {
            Given(() =>
            {
                SetThe<DisposeAction>().To(new DisposeAction(() => _actionIsCalled = true));
            });

            When(() =>
            {
                The<DisposeAction>().Dispose();
            });
        }

        private bool _actionIsCalled;

        [Fact]
        public void Should_Call_Action_When_Disposed()
        {
            _actionIsCalled.ShouldBe(true);
        }
    }
}
