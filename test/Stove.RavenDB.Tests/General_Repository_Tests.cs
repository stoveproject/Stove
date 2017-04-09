using System;

using Shouldly;

using Stove.Domain.Repositories;
using Stove.Domain.Uow;
using Stove.RavenDB.Tests.Entities;

using Xunit;

namespace Stove.RavenDB.Tests
{
    public class General_Repository_Tests : RavenDBTestBase
    {
        public General_Repository_Tests()
        {
            Building(builder => { }).Ok();
        }

        [Fact]
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
            The<IRepository<Product>>().Insert(product);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            Product item = The<IRepository<Product>>().FirstOrDefault(x => x.Name == productName);
            item.ShouldNotBeNull();
        }

        [Fact]
        public void Update_should_work()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            string productName = Guid.NewGuid().ToString("N");
            var product = new Product(productName);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            The<IRepository<Product>>().Insert(product);

            Product item = The<IRepository<Product>>().FirstOrDefault(x => x.Name == productName);
            item.Name = "Pant";
            The<IRepository<Product>>().Update(item);
            
            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            Product pant = The<IRepository<Product>>().FirstOrDefault(x => x.Name == "Pant");
            pant.Name.ShouldBe("Pant");
            pant.ShouldNotBeNull();
        }

        [Fact]
        public void Update_should_work_with_object_tracking_mechanism()
        {
            using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin())
            {
                //-----------------------------------------------------------------------------------------------------------
                // Arrange
                //-----------------------------------------------------------------------------------------------------------
                var product = new Product("ThreeTShirt");

                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                int insertedId = The<IRepository<Product>>().InsertAndGetId(product);

                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                Product item = The<IRepository<Product>>().Get(insertedId);
                item.Name = "Pant";

                Product pant = The<IRepository<Product>>().FirstOrDefault(x => x.Id == item.Id);
                pant.ShouldNotBeNull();
                pant.Name.ShouldBe("Pant");

                uow.Complete();
            }
        }
    }
}
