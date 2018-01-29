using Shouldly;

using Stove.Domain.Repositories;
using Stove.Domain.Uow;
using Stove.Extensions;
using Stove.Tests.SampleApplication.Domain.Entities;

using Xunit;

namespace Stove.Tests.SampleApplication
{
    public class BasicRepository_Tests : SampleApplicationTestBase
    {
        public BasicRepository_Tests()
        {
            Building(builder => { }).Ok();
        }

        [Fact]
        public void firstordefault_should_work()
        {
            var uowManager = The<IUnitOfWorkManager>();
            var userRepository = The<IRepository<User>>();

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
            var uowManager = The<IUnitOfWorkManager>();
            var userRepository = The<IRepository<User>>();

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

        [Fact]
        public void uow_complete_handle_eventbus_should_work_with_repository_insert()
        {
            var uowManager = The<IUnitOfWorkManager>();
            var userRepository = The<IRepository<User>>();

            using (IUnitOfWorkCompleteHandle uow = uowManager.Begin())
            {
                for (var i = 0; i < 1000; i++)
                {
                    userRepository.Insert(new User
                    {
                        Email = "ouzsykn@hotmail.com",
                        Surname = "Sykn",
                        Name = "Oğuz"
                    });
                }

                uow.Complete();
            }

            using (IUnitOfWorkCompleteHandle uow = uowManager.Begin())
            {
                userRepository.GetAll().ForEach(user => user.Surname = "Soykan");
                userRepository.Count(x => x.Email == "ouzsykn@hotmail.com").ShouldBe(1000);
                userRepository.FirstOrDefault(x => x.Email == "ouzsykn@hotmail.com").ShouldNotBeNull();

                uow.Complete();
            }
        }

        [Fact]
        public void uow_complete_handle_eventbus_should_work_with_repository_insert2()
        {
            var uowManager = The<IUnitOfWorkManager>();
            var userRepository = The<IRepository<User>>();

            using (IUnitOfWorkCompleteHandle uow = uowManager.Begin())
            {
                for (var i = 0; i < 1000; i++)
                {
                    userRepository.Insert(new User
                    {
                        Email = "ouzsykn@hotmail.com",
                        Surname = "Sykn",
                        Name = "Oğuz"
                    });
                }

                uow.Complete();
            }

            using (IUnitOfWorkCompleteHandle uow = uowManager.Begin())
            {
                userRepository.GetAll().ForEach(user => user.Surname = "Soykan");
                userRepository.Count(x => x.Email == "ouzsykn@hotmail.com").ShouldBe(1000);
                userRepository.FirstOrDefault(x => x.Email == "ouzsykn@hotmail.com").ShouldNotBeNull();

                uow.Complete();
            }
        }
    }
}
