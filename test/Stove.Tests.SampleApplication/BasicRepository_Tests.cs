using Shouldly;

using Stove.Domain.Repositories;
using Stove.Domain.Uow;
using Stove.Tests.SampleApplication.Domain.Entities;

using Xunit;

namespace Stove.Tests.SampleApplication
{
    public class BasicRepository_Tests : SampleApplicationTestBase<SampleApplicationBootstrapper>
    {
        public BasicRepository_Tests()
        {
            Building(builder => { }).Ok();

            CreateInitialData();
        }

        [Fact]
        public void firstordefault_should_work()
        {
            var uowManager = LocalResolver.Resolve<IUnitOfWorkManager>();
            var userRepository = LocalResolver.Resolve<IRepository<User>>();

            using (IUnitOfWorkCompleteHandle uow = uowManager.Begin())
            {
                User user = userRepository.FirstOrDefault(x => x.Name == "Oğuzhan");
                user.ShouldNotBeNull();

                uow.Complete();
            }
        }

        [Fact]
        public void uow_rollback_should_work_with_repository_insert()
        {
            var uowManager = LocalResolver.Resolve<IUnitOfWorkManager>();
            var userRepository = LocalResolver.Resolve<IRepository<User>>();

            using (IUnitOfWorkCompleteHandle uow = uowManager.Begin())
            {
                userRepository.Insert(new User
                {
                    Email = "ouzsykn@hotmail.com",
                    Surname = "Sykn",
                    Name = "Oğuz"
                });

                //not complete, should rollback!
            }

            using (IUnitOfWorkCompleteHandle uow = uowManager.Begin())
            {
                userRepository.FirstOrDefault(x => x.Email == "ouzsykn@hotmail.com").ShouldBeNull();
            }
        }
    }
}
