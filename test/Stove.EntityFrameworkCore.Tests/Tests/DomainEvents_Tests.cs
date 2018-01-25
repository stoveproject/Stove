using Shouldly;

using Stove.Domain.Repositories;
using Stove.EntityFrameworkCore.Tests.Domain;
using Stove.Events.Bus;

using Xunit;

namespace Stove.EntityFrameworkCore.Tests.Tests
{
	public class DomainEvents_Tests : EntityFrameworkCoreTestBase
	{
		private readonly IRepository<Blog> _blogRepository;
		private readonly IEventBus _eventBus;

		public DomainEvents_Tests()
		{
			Building(builder => { }).Ok();

			_blogRepository = The<IRepository<Blog>>();
			_eventBus = The<IEventBus>();
		}

		[Fact]
		public void Should_Trigger_Domain_Events_For_Aggregate_Root()
		{
			//Arrange

			var isTriggered = false;

			_eventBus.Register<BlogUrlChangedEvent>(data =>
			{
				data.Url.ShouldBe("http://testblog1-changed.myblogs.com");
				isTriggered = true;
			});

			//Act

			Blog blog1 = _blogRepository.Single(b => b.Name == "test-blog-1");
			blog1.ChangeUrl("http://testblog1-changed.myblogs.com");
			_blogRepository.Update(blog1);

			//Assert

			isTriggered.ShouldBeTrue();
		}
	}
}
