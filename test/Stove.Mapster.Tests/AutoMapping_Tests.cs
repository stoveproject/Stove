using System;

using JetBrains.Annotations;

using Mapster;

using Shouldly;

using Stove.Mapster.Mapster;
using Stove.ObjectMapping;

using Xunit;

namespace Stove.Mapster.Tests
{
    public class AutoMapping_Tests : StoveMapsterTestApplication
    {
        public AutoMapping_Tests()
        {
            Building(builder => { builder.UseStoveMapster(); }).Ok();
        }

        [Fact]
        public void mapping_should_work_with_classic_way_when_only_TDestination_provided()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            //TypeAdapterConfig.GlobalSettings.RequireExplicitMapping = true;
            //TypeAdapterConfig<MyClass, MyClassDto>.NewConfig();
            var mapper = LocalResolver.Resolve<IObjectMapper>();

            var myclass = new MyClass { TestProperty = "Oguzhan" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            var mappedObject = mapper.Map<MyClassDto>(myclass);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            mappedObject.TestProperty.ShouldNotBeNull();
            mappedObject.TestProperty.ShouldBe("Oguzhan");
        }

        [Fact]
        public void auto_attibute_mapping_should_work_when_only_TDestination_provided()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            //TypeAdapterConfig.GlobalSettings.RequireExplicitMapping = true;
            //TypeAdapterConfig.GlobalSettings.CreateAutoAttributeMaps(typeof(MyClass));
            var mapper = LocalResolver.Resolve<IObjectMapper>();

            var myclass = new MyClass { TestProperty = "Oguzhan" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            var mappedObject = mapper.Map<MyClassDto>(myclass);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            mappedObject.TestProperty.ShouldNotBeNull();
            mappedObject.TestProperty.ShouldBe("Oguzhan");
        }

        [Fact]
        public void auto_attibute_mapping_should_work_when_only_TDestination_and_TSource_provided()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            //TypeAdapterConfig.GlobalSettings.RequireExplicitMapping = true;
            //TypeAdapterConfig.GlobalSettings.CreateAutoAttributeMaps(typeof(MyClass));
            var mapper = LocalResolver.Resolve<IObjectMapper>();

            var myclass = new MyClass { TestProperty = "Oguzhan" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            MyClassDto mappedObject = mapper.Map(myclass, new MyClassDto());

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            mappedObject.TestProperty.ShouldNotBeNull();
            mappedObject.TestProperty.ShouldBe("Oguzhan");
        }

        [Fact]
        public void auto_attibute_mapping_should_throw_CompileException_when_target_types_are_null()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            //TypeAdapterConfig.GlobalSettings.RequireExplicitMapping = true;
            //TypeAdapterConfig.GlobalSettings.CreateAutoAttributeMaps(typeof(MyClassTargetTypeNull));
            var mapper = LocalResolver.Resolve<IObjectMapper>();

            var myclass = new MyClassTargetTypeNull { TestProperty = "Oguzhan" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action mappingAction = () => { mapper.Map(myclass, new MyClassDto()); };

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            mappingAction.ShouldThrow<CompileException>();
        }

        public void auto_attribute_type_should_work_with_multiple_attributes_when_TSource_and_TDestination_provided()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            //TypeAdapterConfig.GlobalSettings.RequireExplicitMapping = true;
            //TypeAdapterConfig.GlobalSettings.CreateAutoAttributeMaps(typeof(MyClassWithMultipleAttribute));
            var mapper = LocalResolver.Resolve<IObjectMapper>();

            var myclass = new MyClassWithMultipleAttribute { TestProperty = "Oguzhan" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            MyClassDto mappedObject1 = mapper.Map(myclass, new MyClassDto());
            MyClassDto2 mappedObject2 = mapper.Map(myclass, new MyClassDto2());

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            mappedObject1.ShouldNotBeNull();
            mappedObject2.ShouldNotBeNull();
            mappedObject1.TestProperty.ShouldBe("Oguzhan");
            mappedObject2.TestProperty.ShouldBe("Oguzhan");
        }

        [UsedImplicitly]
        [AutoMapFrom(typeof(MyClassDto))]
        public class MyClass
        {
            public string TestProperty { get; set; }
        }

        [AutoMapFrom(typeof(MyClassDto), typeof(MyClassDto2))]
        public class MyClassWithMultipleAttribute
        {
            public string TestProperty { get; set; }
        }

        [AutoMapFrom(null)]
        public class MyClassTargetTypeNull
        {
            public string TestProperty { get; set; }
        }

        [UsedImplicitly]
        public class MyClassDto
        {
            public string TestProperty { get; set; }
        }

        public class MyClassDto2
        {
            public string TestProperty { get; set; }
        }
    }
}
