using System;

using Shouldly;

using Stove.Domain.Repositories;
using Stove.EntityFrameworkCore.Tests.Domain;
using Stove.Timing;

using Xunit;

namespace Stove.EntityFrameworkCore.Tests.Tests
{
    public class EntityUtcDateTime_Tests : EntityFrameworkCoreTestBase
    {
        private readonly IRepository<Blog> _blogRepository;

        public EntityUtcDateTime_Tests()
        {
	        Building(builder => { }).Ok();

			_blogRepository = The<IRepository<Blog>>();
        }

        [Fact]
        public void DateTimes_Should_Be_UTC()
        {
            Clock.Kind.ShouldBe(DateTimeKind.Utc);

            //Act

            var blogs = _blogRepository.GetAllList();

            //Assert

            blogs.Count.ShouldBeGreaterThan(0);

            foreach (var blog in blogs)
            {
                blog.CreationTime.Kind.ShouldBe(DateTimeKind.Utc);
            }
        }
    }
}
