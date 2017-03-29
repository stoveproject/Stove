using Shouldly;

using Stove.Domain.Uow;
using Stove.Tests.SampleApplication.Domain.Entities;

using Xunit;

namespace Stove.Tests.SampleApplication.Application.Users
{
    public class UserAppService_Tests : SampleApplicationTestBase
    {
        public UserAppService_Tests()
        {
            Building(builder => { }).Ok();
        }

        [Fact]
        public void ApplicationService_should_work_with_repository()
        {
            using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin())
            {
                var userAppService = The<IUserAppService>();

                User user = userAppService.GetUserByName("Oğuzhan");

                user.ShouldNotBeNull();

                uow.Complete();
            }
        }
    }
}
