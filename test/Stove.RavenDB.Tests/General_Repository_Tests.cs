using Shouldly;

using Stove.Domain.Repositories;
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
            var product = new Product("TShirt");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            The<IRepository<Product>>().Insert(product);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------

            The<IRepository<Product>>().Count().ShouldBe(1);
        }
    }
}
