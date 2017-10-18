using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Shouldly;

using Stove.Domain.Repositories;
using Stove.Domain.Uow;
using Stove.Events.Bus;
using Stove.Events.Bus.Entities;
using Stove.NHibernate.Tests.Entities;

using Xunit;

namespace Stove.NHibernate.Tests
{
    public class General_Repository_Tests : StoveNHibernateTestBase
    {
        public General_Repository_Tests()
        {
            Building(builder => { }).Ok();
            StoveSession.UserId = 1;
            UsingSession(session => { session.Save(new Product("TShirt")); });
        }

        [Fact]
        public void GetAll_should_work()
        {
            using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin())
            {
                var repository = The<IRepository<Product>>();
                repository.GetAll().Count().ShouldBe(1);
                repository.GetAllList().Count.ShouldBe(1);

                uow.Complete();
            }
        }

        [Fact]
        public void Insert_should_work()
        {
            using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin())
            {
                The<IRepository<Product>>().Insert(new Product("Pants"));

                Product product = UsingSession(session => session.Query<Product>().FirstOrDefault(p => p.Name == "Pants"));
                product.ShouldNotBe(null);
                product.IsTransient().ShouldBe(false);
                product.Name.ShouldBe("Pants");

                uow.Complete();
            }
        }

        [Fact]
        public void Insert_should_work_on_multi_database()
        {
            using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin())
            {
                The<IRepository<Product>>().Insert(new Product("Pants"));
                The<IRepository<Category>>().Insert(new Category("Bread"));

                Product product = UsingSession(session => session.Query<Product>().FirstOrDefault(p => p.Name == "Pants"));
                product.ShouldNotBe(null);
                product.IsTransient().ShouldBe(false);
                product.Name.ShouldBe("Pants");

                Category category = The<IRepository<Category>>().FirstOrDefault(x => x.Name == "Bread");
                category.ShouldNotBe(null);
                category.IsTransient().ShouldBe(false);
                category.Name.ShouldBe("Bread");

                uow.Complete();
            }
        }

        [Fact]
        public void Update_With_Action_Test()
        {
            using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin())
            {
                Product productBefore = UsingSession(session => session.Query<Product>().Single(p => p.Name == "TShirt"));

                Product updatedUser = The<IRepository<Product>>().Update(productBefore.Id, user => user.Name = "Polo");
                updatedUser.Id.ShouldBe(productBefore.Id);
                updatedUser.Name.ShouldBe("Polo");

                The<IUnitOfWorkManager>().Current.SaveChanges();

                Product productAfter = UsingSession(session => session.Get<Product>(productBefore.Id));
                productAfter.Name.ShouldBe("Polo");

                uow.Complete();
            }
        }

        [Fact]
        public void Should_Trigger_Event_On_Insert()
        {
            using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin())
            {
                var triggerCount = 0;

                The<IEventBus>().Register<EntityCreatedEventData<Product>>(
                    eventData =>
                    {
                        eventData.Entity.Name.ShouldBe("Kazak");
                        eventData.Entity.IsTransient().ShouldBe(false);
                        triggerCount++;
                    });

                The<IRepository<Product>>().Insert(new Product("Kazak"));

                uow.Complete();

                triggerCount.ShouldBe(1);
            }
        }

        [Fact]
        public void Should_Trigger_Event_On_Update()
        {
            using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin())
            {
                var triggerCount = 0;

                The<IEventBus>().Register<EntityUpdatedEventData<Product>>(
                    eventData =>
                    {
                        eventData.Entity.Name.ShouldBe("Kazak");
                        triggerCount++;
                    });

                Product product = The<IRepository<Product>>().Single(p => p.Name == "TShirt");
                product.Name = "Kazak";
                The<IRepository<Product>>().Update(product);

                uow.Complete();

                triggerCount.ShouldBe(1);
            }
        }

        [Fact]
        public void Should_Trigger_Event_On_Delete()
        {
            using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin())
            {
                var triggerCount = 0;

                The<IEventBus>().Register<EntityDeletedEventData<Product>>(
                    eventData =>
                    {
                        eventData.Entity.Name.ShouldBe("TShirt");
                        triggerCount++;
                    });

                Product product = The<IRepository<Product>>().Single(p => p.Name == "TShirt");
                The<IRepository<Product>>().Delete(product.Id);

                The<IRepository<Product>>().FirstOrDefault(p => p.Name == "TShirt").ShouldBe(null);

                uow.Complete();

                triggerCount.ShouldBe(1);
            }
        }

        [Fact]
        public async Task Should_rollback_when_CancellationToken_cancel_is_requested()
        {
            var ts = new CancellationTokenSource();
            int updatingEventTriggerCount = 0;
            try
            {
                using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin())
                {
                    The<IEventBus>().Register<EntityUpdatingEventData<Product>>(
                        eventData =>
                        {
                            eventData.Entity.Name.ShouldBe("Pants");
                            ts.Cancel(true);
                            updatingEventTriggerCount++;
                        });

                    Product product = The<IRepository<Product>>().Single(p => p.Name == "TShirt");

                    product.Name = "Pants";

                    await uow.CompleteAsync(ts.Token);
                }
            }
            catch (Exception exception)
            {
                //Handle
            }

            using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin())
            {
                The<IRepository<Product>>().FirstOrDefault(x => x.Name == "Pants").ShouldBeNull();

                await uow.CompleteAsync();
            }

            updatingEventTriggerCount.ShouldBe(1);
        }
    }
}
