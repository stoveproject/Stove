using System;

using Shouldly;

using Stove.Couchbase.Tests.Entities;
using Stove.Domain.Repositories;
using Stove.Domain.Uow;
using Stove.Events.Bus;
using Stove.Events.Bus.Entities;

using Xunit;

namespace Stove.Couchbase.Tests
{
    public class General_Repository_Tests : CouchbaseTestBase
    {
        public General_Repository_Tests()
        {
            Building(builder => { }).Ok();
        }

        [Fact(Skip = "Flaky")]
        public void Insert_should_work()
        {

            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            string productName = Guid.NewGuid().ToString("N");
            var product = new Product(productName);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Product inserted = The<IRepository<Product, string>>().Insert(product);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            Product item = The<IRepository<Product, string>>().FirstOrDefault(x => x.Id == inserted.Id);
            item.ShouldNotBeNull();
        }

        [Fact(Skip = "Flaky")]
        public void Insert_and_event_fire_should_work()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            string productName = Guid.NewGuid().ToString("N");
            var product = new Product(productName);
            var eventInvocationCount = 0;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------

            //The<IEventBus>().Register<EntityCreatingEvent<Product>>(data =>
            //{
            //    eventInvocationCount++;
            //});

            Product inserted = The<IRepository<Product, string>>().Insert(product);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            Product item = The<IRepository<Product, string>>().FirstOrDefault(x => x.Id == inserted.Id);
            item.ShouldNotBeNull();
            eventInvocationCount.ShouldBe(1);
        }

        [Fact(Skip = "Flaky")]
        public void Update_should_work()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            string productName = Guid.NewGuid().ToString("N");
            var product = new Product(productName);

            string updateName = Guid.NewGuid().ToString("N");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            The<IRepository<Product, string>>().Insert(product);

            Product item = The<IRepository<Product, string>>().FirstOrDefault(x => x.Name == productName);
            item.Name = updateName;
            The<IRepository<Product, string>>().Update(item);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            Product pant = The<IRepository<Product, string>>().FirstOrDefault(x => x.Name == updateName);
            pant.Name.ShouldBe(updateName);
            pant.ShouldNotBeNull();
        }

        [Fact(Skip = "Flaky")]
        public void Update_should_work_with_object_tracking_mechanism()
        {
            using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin())
            {
                //-----------------------------------------------------------------------------------------------------------
                // Arrange
                //-----------------------------------------------------------------------------------------------------------
                string productName = Guid.NewGuid().ToString("N");
                var product = new Product(productName);

                string updateName = Guid.NewGuid().ToString("N");

                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                The<IRepository<Product, string>>().Insert(product);

                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                Product item = The<IRepository<Product, string>>().FirstOrDefault(x => x.Name == productName);
                item.Name = updateName;

                The<IUnitOfWorkManager>().Current.SaveChanges();

                Product pant = The<IRepository<Product, string>>().FirstOrDefault(x => x.Name == item.Name);
                pant.ShouldNotBeNull();
                pant.Name.ShouldBe(updateName);

                uow.Complete();
            }
        }

        [Fact(Skip = "Flaky")]
        public void Delete_should_work_on_hard_deletable_entities()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            string productName = Guid.NewGuid().ToString("N");
            var product = new Product(productName);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Product inserted = The<IRepository<Product, string>>().Insert(product);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            The<IRepository<Product, string>>().Delete(inserted);

            Product deleted = The<IRepository<Product, string>>().FirstOrDefault(x => x.Name == productName);
            deleted.ShouldBeNull();
        }

        [Fact(Skip = "Flaky")]
        public void Delete_should_work_soft_deletable_entities()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            string productName = Guid.NewGuid().ToString("N");
            string address = Guid.NewGuid().ToString("N");
            var product = new Product(productName);
            var order = new Order(address, product);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Order insertedOrder = The<IRepository<Order, string>>().Insert(order);

            The<IRepository<Order, string>>().Delete(insertedOrder);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            insertedOrder.IsDeleted.ShouldBe(true);
        }
    }
}
