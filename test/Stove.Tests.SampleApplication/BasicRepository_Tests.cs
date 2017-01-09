using Shouldly;

using Stove.Domain.Repositories;
using Stove.Domain.Uow;
using Stove.Tests.SampleApplication.Domain.Entities;
using Stove.Timing;

using Xunit;

namespace Stove.Tests.SampleApplication
{
    public class BasicRepository_Tests : TestBase
    {
        public BasicRepository_Tests()
        {
            Building(builder => { }).Ok();

            UsingDbContext(context =>
            {
                context.Users.Add(new User
                {
                    CreationTime = Clock.Now,
                    Id = 1,
                    Name = "Oğuzhan"
                });
            });
        }

        [Fact]
        public void Get_Should_Work()
        {
            var uowManager = LocalResolver.Resolve<IUnitOfWorkManager>();
            var userRepository = LocalResolver.Resolve<IRepository<User, long>>();

            using (IUnitOfWorkCompleteHandle uow = uowManager.Begin())
            {
                userRepository.FirstOrDefault(x => x.Name == "Oğuzhan").ShouldNotBeNull();

                uow.Complete();
            }
        }
    }
}
