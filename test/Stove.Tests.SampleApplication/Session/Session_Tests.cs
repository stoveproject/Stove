using Shouldly;

using Stove.Runtime.Session;

using Xunit;

namespace Stove.Tests.SampleApplication.Session
{
    public class Session_Tests : SampleApplicationTestBase
    {
        private readonly IStoveSession _session;

        public Session_Tests()
        {
            Building(builder => { }).Ok();

            _session = LocalResolver.Resolve<IStoveSession>();
        }

        [Fact]
        public void Session_Override_Test()
        {
            _session.UserId.ShouldBeNull();

            using (_session.Use(571))
            {
                using (_session.Use(3))
                {
                    _session.UserId.ShouldBe(3);
                }

                _session.UserId.ShouldBe(571);
            }

            _session.UserId.ShouldBeNull();
        }
    }
}
