using System;

using JetBrains.Annotations;

using Mapster;

using Shouldly;

using Stove.Configuration;
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
        public void adapter_should_not_be_null()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IStoveMapsterConfiguration mapsterConfiguration = The<IModuleConfigurations>().StoveMapster();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            IAdapter adapter = mapsterConfiguration.Adapter;

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            adapter.ShouldNotBeNull();
        }

        [Fact]
        public void mapping_should_work_with_classic_way_when_only_TDestination_provided()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var mapper = The<IObjectMapper>();

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
            var mapper = The<IObjectMapper>();

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
            var mapper = The<IObjectMapper>();

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
        public void auto_attibute_mapping_should_throw_CompileException_when_target_types_are_null_while_using_From_aspect()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var mapper = The<IObjectMapper>();

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

        [Fact]
        public void auto_attibute_mapping_should_throw_CompileException_when_target_types_are_null_while_using_two_way_aspect()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var mapper = The<IObjectMapper>();

            var myclass = new MyClassTargetTypeNullTwoWay { TestProperty = "Oguzhan" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action mappingAction = () => { mapper.Map(myclass, new MyClassDto()); };

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            mappingAction.ShouldThrow<CompileException>();
        }

        [Fact]
        public void auto_attribute_type_should_work_with_multiple_attributes_when_TSource_and_TDestination_provided()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var mapper = The<IObjectMapper>();

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

        [Fact]
        public void auto_attribute_type_should_work_using_two_way_aspect()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var mapper = The<IObjectMapper>();
            var myclass = new MyClassTwoWay { TestProperty = "Oguzhan" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            MyClassDto mappedObject = mapper.Map(myclass, new MyClassDto());
            MyClassTwoWay twoWayMappedObject = mapper.Map(mappedObject, new MyClassTwoWay());

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            mappedObject.ShouldNotBeNull();
            mappedObject.TestProperty.ShouldBe("Oguzhan");
            twoWayMappedObject.ShouldNotBeNull();
            twoWayMappedObject.TestProperty.ShouldBe("Oguzhan");
        }

        [Fact]
        public void auto_attribute_type_should_work_using_two_way_aspect_with_multiple_types()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var mapper = The<IObjectMapper>();
            var myclass = new MyClassTwoWayMultipleTypes { TestProperty = "Oguzhan" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            MyClassDto mappedObject = mapper.Map(myclass, new MyClassDto());
            MyClassDto2 mappedObject2 = mapper.Map(myclass, new MyClassDto2());
            MyClassTwoWayMultipleTypes twoWayMappedObject = mapper.Map(mappedObject, new MyClassTwoWayMultipleTypes());
            MyClassTwoWayMultipleTypes twoWayMappedObject2 = mapper.Map(mappedObject2, new MyClassTwoWayMultipleTypes());

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            mappedObject.ShouldNotBeNull();
            mappedObject.TestProperty.ShouldBe("Oguzhan");
            twoWayMappedObject.ShouldNotBeNull();
            twoWayMappedObject.TestProperty.ShouldBe("Oguzhan");
            mappedObject2.ShouldNotBeNull();
            mappedObject2.TestProperty.ShouldBe("Oguzhan");
            twoWayMappedObject2.ShouldNotBeNull();
            twoWayMappedObject2.TestProperty.ShouldBe("Oguzhan");
        }

        [Fact]
        public void auto_attribute_type_should_work_with_multiple_attributes_using_to_aspect()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var mapper = The<IObjectMapper>();
            var myclassDto = new MyClassDto { TestProperty = "Oguzhan" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            var mappedObject1 = mapper.Map<MyClassToAspect1>(myclassDto);
            var mappedObject2 = mapper.Map<MyClassToAspect2>(myclassDto);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            mappedObject1.ShouldNotBeNull();
            mappedObject1.TestProperty.ShouldBe("Oguzhan");
            mappedObject2.ShouldNotBeNull();
            mappedObject2.TestProperty.ShouldBe("Oguzhan");
        }

        [Fact]
        public void auto_attribute_type_should_throw_CompileException_with_null_types_using_to_aspect()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var mapper = The<IObjectMapper>();
            var myclassDto = new MyClassDtoNullTypes { TestProperty = "Oguzhan" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action mappingAction = () => { mapper.Map<MyClassToAspect1>(myclassDto); };

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            mappingAction.ShouldThrow<CompileException>();
        }

        [Fact]
        public void mapping_should_work_with_extensions()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var myclass = new MyClass { TestProperty = "Oguzhan" };
            var myclassDto = new MyClassDto();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            var mappedObject = myclass.MapTo<MyClassDto>();
            var mappedObject2 = myclass.MapTo(myclassDto);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            mappedObject.ShouldNotBeNull();
            mappedObject.TestProperty.ShouldBe("Oguzhan");
            mappedObject2.ShouldNotBeNull();
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

        [AutoMap(typeof(MyClassDto))]
        public class MyClassTwoWay
        {
            public string TestProperty { get; set; }
        }

        public class MyClassToAspect1
        {
            public string TestProperty { get; set; }
        }

        public class MyClassToAspect2
        {
            public string TestProperty { get; set; }
        }

        [AutoMap(typeof(MyClassDto), typeof(MyClassDto2))]
        public class MyClassTwoWayMultipleTypes
        {
            public string TestProperty { get; set; }
        }

        [AutoMapFrom(null)]
        public class MyClassTargetTypeNull
        {
            public string TestProperty { get; set; }
        }

        [AutoMap(null)]
        public class MyClassTargetTypeNullTwoWay
        {
            public string TestProperty { get; set; }
        }

        [AutoMapTo(typeof(MyClassToAspect1), typeof(MyClassToAspect2))]
        public class MyClassDto
        {
            public string TestProperty { get; set; }
        }

        [AutoMapTo(null)]
        public class MyClassDtoNullTypes
        {
            public string TestProperty { get; set; }
        }

        public class MyClassDto2
        {
            public string TestProperty { get; set; }
        }
    }
}
