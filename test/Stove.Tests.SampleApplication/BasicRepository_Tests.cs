using System;
using System.Threading.Tasks;

using Autofac.Extras.IocManager;

using Shouldly;

using Stove.Domain.Repositories;
using Stove.Domain.Uow;
using Stove.Events.Bus;
using Stove.Events.Bus.Entities;
using Stove.Events.Bus.Handlers;
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

        [Fact]
        public void uow_completed_event_should_fire_when_uow_is_completed()
        {
            var executionCount = 0;
            The<IEventBus>().Register<UserCretedEventAfterUowCompleted>(completed =>
            {
                executionCount++;
            });

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

                The<IUnitOfWorkCompletedEventHelper>().Trigger(new UserCretedEventAfterUowCompleted() { Name = "Oğuz" });

                uow.Complete();
            }

            executionCount.ShouldBe(1);

        }

        [Fact]
        public async Task uow_completed_async_event_should_fire_when_uow_is_completed()
        {
            var executionCount = 0;
            The<IEventBus>().Register<UserCretedEventAfterUowCompleted>(async completed =>
            {
                executionCount++;
                await Task.FromResult(0);
            });
            var uowManager = The<IUnitOfWorkManager>();
            var userRepository = The<IRepository<User>>();

            using (IUnitOfWorkCompleteHandle uow = uowManager.Begin())
            {
                await userRepository.InsertAsync(new User
                {
                    Email = "ouzsykn@hotmail.com",
                    Surname = "Sykn",
                    Name = "Oğuz"
                });

                await The<IUnitOfWorkCompletedEventHelper>().TriggerAsync(new UserCretedEventAfterUowCompleted() { Name = "Oğuz" });

                await uow.CompleteAsync();
            }

            executionCount.ShouldBe(1);
        }

        [Fact]
        public void uow_completed_event_should_not_fire_when_uow_is_not_completed()
        {
            var executionCount = 0;
            The<IEventBus>().Register<UserCretedEventAfterUowCompleted>(completed =>
            {
                executionCount++;
            });
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

                The<IUnitOfWorkCompletedEventHelper>().Trigger(new UserCretedEventAfterUowCompleted() { Name = "Oğuz" });
            }

            executionCount.ShouldBe(0);
        }

        public class UserCreatedEventHandler : IEventHandler<EntityCreatedEventData<User>>,
            IEventHandler<EntityUpdatedEventData<User>>,
            ITransientDependency
        {
            public void HandleEvent(EntityCreatedEventData<User> eventData)
            {
                User a = eventData.Entity;
            }

            public void HandleEvent(EntityUpdatedEventData<User> eventData)
            {
                User a = eventData.Entity;
            }
        }

        public class UserCretedEventAfterUowCompleted : EventData
        {
            public string Name { get; set; }
        }
    }
}

