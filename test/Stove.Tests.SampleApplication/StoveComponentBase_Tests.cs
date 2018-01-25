using System.Threading.Tasks;

using Shouldly;

using Stove.Tests.SampleApplication;
using Stove.Tests.SampleApplication.Domain;
using Stove.Tests.SampleApplication.Domain.Entities;

using Xunit;

namespace Stove.Tests
{
    public class StoveComponentBase_Tests : SampleApplicationTestBase
    {
        public StoveComponentBase_Tests()
        {
            Building(builder => { }).Ok();
        }

        [Fact]
        public void UseUow_should_work()
        {
            User user = The<SomeDomainService>().GetUserByName("Oğuzhan");
            user.ShouldNotBeNull();
        }

        [Fact]
        public async Task UseUow_async_should_work()
        {
            User user = await The<SomeDomainService>().GetUserByName_async("Oğuzhan");
            user.ShouldNotBeNull();
        }

        [Fact]
        public async Task UseUow_with_IsolationLevel_should_work()
        {
            User user = await The<SomeDomainService>().GetUserByName_async_With_IsolationLevel("Oğuzhan");
            user.ShouldNotBeNull();
        }

        [Fact]
        public void UseUow_isTransactional_should_work()
        {
            User user = The<SomeDomainService>().GetUserByName_isTransactional("Oğuzhan");
            user.ShouldNotBeNull();
        }

        [Fact]
        public void UseUow_isolationLevel_should_work()
        {
            User user = The<SomeDomainService>().GetUserByName_with_isolationlevel("Oğuzhan");
            user.ShouldNotBeNull();
        }

        [Fact]
        public async Task UseUow_async_isTransactional_should_work()
        {
            User user = await The<SomeDomainService>().GetUserByName_async_isTransactional("Oğuzhan");
            user.ShouldNotBeNull();
        }

        [Fact]
        public async Task UseUow_and_auditing()
        {
            using (StoveSession.Use(266))
            {
                Message message = await The<SomeDomainService>().CreateMessageAndGet("message");
                message.Subject.ShouldBe("message");
                message.CreatorUserId.ShouldBe(266);
            }
        }
    }
}
