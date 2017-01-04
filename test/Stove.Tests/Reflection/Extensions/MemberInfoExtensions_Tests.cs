using System;

using Shouldly;

using Stove.Reflection.Extensions;

using Xunit;

namespace Stove.Tests.Reflection.Extensions
{
    public class MemberInfoExtensions_Tests
    {
        [Theory]
        [InlineData(typeof(MyClass))]
        [InlineData(typeof(MyBaseClass))]
        public void GetSingleAttributeOfTypeOrBaseTypesOrNull_Test(Type type)
        {
            var attr = type.GetSingleAttributeOfTypeOrBaseTypesOrNull<MyAttribute>();
            attr.ShouldNotBeNull();
        }

        private class MyClass : MyBaseClass
        {
        }

        [My]
        private abstract class MyBaseClass
        {
        }

        private class MyAttribute : Attribute
        {
        }
    }
}
