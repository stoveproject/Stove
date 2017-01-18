using System;

using Shouldly;

using Stove.Domain.Entities;

using Xunit;

namespace Stove.Tests.Domain.Entities
{
    public class EntityHelper_Tests
    {
        [Fact]
        public void Get_primary_key_type_tests()
        {
            EntityHelper.GetPrimaryKeyType<Manager>().ShouldBe(typeof(int));
            EntityHelper.GetPrimaryKeyType(typeof(Manager)).ShouldBe(typeof(int));
            EntityHelper.GetPrimaryKeyType(typeof(TestEntityWithGuidPk)).ShouldBe(typeof(Guid));
        }

        private class TestEntityWithGuidPk : Entity<Guid>
        {

        }
    }
}