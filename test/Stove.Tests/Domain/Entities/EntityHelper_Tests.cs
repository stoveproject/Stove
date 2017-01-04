using System;

using Shouldly;

using Stove.Domain.Entities;

using Xunit;

namespace Stove.Tests.Domain.Entities
{
    public class EntityHelper_Tests
    {
        [Fact]
        public void GetPrimaryKeyType_Tests()
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