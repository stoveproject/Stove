using System;
using System.Linq;

using Shouldly;

using Stove.Dapper;
using Stove.Dapper.Repositories;
using Stove.Domain.Repositories;
using Stove.Domain.Uow;
using Stove.Events.Bus;
using Stove.Events.Bus.Entities;
using Stove.NHibernate.Tests.Entities;
using Stove.NHibernate.Tests.Sessions;

using Xunit;

namespace Stove.NHibernate.Tests
{
    public class General_Repository_With_Dapper_Tests : StoveNHibernateTestBase
    {
        public General_Repository_With_Dapper_Tests()
        {
            Building(builder => { builder.UseStoveDapper(); }).Ok();

            StoveSession.UserId = 1;
            UsingSession<PrimaryStoveSessionContext>(session => session.Save(new Product("TShirt")));
        }

        [Fact]
        public void Dapper_And_NH_should_be_able_to_work_under_same_tranaction()
        {
            using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin())
            {
                The<IDapperRepository<Product>>().GetAll().Count().ShouldBe(1);
                The<IRepository<Product>>().GetAll().Count().ShouldBe(1);

                uow.Complete();
            }
        }

        [Fact]
        public void Dapper_and_NH_should_return_same_object()
        {
            using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin())
            {
                Product productFromDapper = The<IDapperRepository<Product>>().FirstOrDefault(x => x.Name == "TShirt");
                Product productFromNh = The<IRepository<Product>>().FirstOrDefault(x => x.Name == "TShirt");

                productFromNh.Id.ShouldBe(productFromDapper.Id);

                uow.Complete();
            }
        }

        [Fact]
        public void If_Dapper_inserts_an_entity_Nh_should_know_that()
        {
            using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin())
            {
                The<IDapperRepository<Product>>().Insert(new Product("ProductFromDapper"));
                Product productFromNhButInsertedByDapper = The<IRepository<Product>>().FirstOrDefault(x => x.Name == "ProductFromDapper");

                productFromNhButInsertedByDapper.CreatorUserId.ShouldBe(1);
                productFromNhButInsertedByDapper.ShouldNotBeNull();

                uow.Complete();
            }
        }

        [Fact]
        public void Should_Rollback_UOW_In_Updating_Event_NhRepository()
        {
            //Arrange
            var updatingTriggerCount = 0;
            var updatedTriggerCount = 0;

            The<IEventBus>().Register<EntityUpdatingEventData<Product>>(
                eventData =>
                {
                    eventData.Entity.Name.ShouldBe("Bear");
                    updatingTriggerCount++;

                    throw new ApplicationException("A sample exception to rollback the UOW.");
                });

            The<IEventBus>().Register<EntityUpdatedEventData<Product>>(
                eventData =>
                {
                    //Should not come here, since UOW is failed
                    updatedTriggerCount++;
                });

            //Act
            try
            {
                using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin())
                {
                    Product product = The<IRepository<Product>>().Single(p => p.Name == "TShirt");
                    product.Name = "Bear";
                    The<IRepository<Product>>().Update(product);

                    uow.Complete();
                }

                Assert.True(false, "Should not come here since ApplicationException is thrown!");
            }
            catch (ApplicationException)
            {
                //hiding exception
            }

            //Assert
            updatingTriggerCount.ShouldBe(1);
            updatedTriggerCount.ShouldBe(0);

            using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin())
            {
                The<IRepository<Product>>()
                    .FirstOrDefault(p => p.Name == "TShirt")
                    .ShouldNotBeNull(); //should not be changed since we throw exception to rollback the transaction
            }
        }

        [Fact]
        public void Should_Rollback_UOW_In_Updating_Event_DapperRepository()
        {
            //Arrange
            var updatingTriggerCount = 0;
            var updatedTriggerCount = 0;

            The<IEventBus>().Register<EntityUpdatingEventData<Product>>(
                eventData =>
                {
                    eventData.Entity.Name.ShouldBe("Bear");
                    updatingTriggerCount++;

                    throw new ApplicationException("A sample exception to rollback the UOW.");
                });

            The<IEventBus>().Register<EntityUpdatedEventData<Product>>(
                eventData =>
                {
                    //Should not come here, since UOW is failed
                    updatedTriggerCount++;
                });

            //Act
            try
            {
                using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin())
                {
                    Product product = The<IRepository<Product>>().Single(p => p.Name == "TShirt");
                    product.Name = "Bear";
                    The<IDapperRepository<Product>>().Update(product);

                    uow.Complete();
                }

                Assert.True(false, "Should not come here since ApplicationException is thrown!");
            }
            catch (ApplicationException)
            {
                //hiding exception
            }

            //Assert
            updatingTriggerCount.ShouldBe(1);
            updatedTriggerCount.ShouldBe(0);

            using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin())
            {
                The<IDapperRepository<Product>>()
                    .FirstOrDefault(p => p.Name == "TShirt")
                    .ShouldNotBeNull(); //should not be changed since we throw exception to rollback the transaction
            }
        }
    }
}
