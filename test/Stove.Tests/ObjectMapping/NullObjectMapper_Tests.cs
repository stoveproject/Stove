using System;

using Shouldly;

using Stove.ObjectMapping;
using Stove.TestBase;

using Xunit;

namespace Stove.Tests.ObjectMapping
{
    public class NullObjectMapper_Tests : TestBaseWithLocalIocResolver
    {
        public NullObjectMapper_Tests()
        {
            Building(builder => { builder.UseStoveNullObjectMapper(); }).Ok();
        }

        [Fact]
        public void Map_TDestination_should_throw_StoveException()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var mapper = LocalResolver.Resolve<IObjectMapper>();
            var myObj = new MyClass2();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action mappingAction = () => { mapper.Map<MyClass1>(myObj); };

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            mappingAction.ShouldThrow<StoveException>();
        }

        [Fact]
        public void Map_TDestination_TSource_should_throw_StoveException()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var mapper = LocalResolver.Resolve<IObjectMapper>();
            var source = new MyClass1();
            var destination = new MyClass2();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action mappingAction = () => { mapper.Map<MyClass1, MyClass2>(source, destination); };

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            mappingAction.ShouldThrow<StoveException>();
        }

        private class MyClass1
        {
        }

        private class MyClass2
        {
        }
    }
}
