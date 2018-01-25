using System.Threading.Tasks;

using Shouldly;

using Stove.Dapper.Repositories;
using Stove.Domain.Repositories;
using Stove.Domain.Uow;
using Stove.EntityFrameworkCore.Dapper.Tests.Domain;
using Stove.EntityFrameworkCore.Dapper.Tests.Domain.Events;
using Stove.Events.Bus;
using Stove.Events.Bus.Entities;

using Xunit;

namespace Stove.EntityFrameworkCore.Dapper.Tests.Tests
{
	public class DomainEvents_Tests : StoveEfCoreDapperTestApplicationBase
	{
		private readonly IDapperRepository<Blog> _blogDapperRepository;
		private readonly IRepository<Blog> _blogRepository;
		private readonly IEventBus _eventBus;

		public DomainEvents_Tests()
		{
			Building(builder => { }).Ok();

			_blogRepository = The<IRepository<Blog>>();
			_blogDapperRepository = The<IDapperRepository<Blog>>();
			_eventBus = The<IEventBus>();
		}

		[Fact]
		public void Should_Trigger_Domain_Events_For_Aggregate_Root()
		{
			var isTriggered = false;
			using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin())
			{
				_eventBus.Register<BlogUrlChangedEvent>(data =>
				{
					data.Url.ShouldBe("http://testblog1-changed.myblogs.com");
					isTriggered = true;
				});

				//Act

				Blog blog1 = _blogRepository.Single(b => b.Name == "test-blog-1");
				blog1.ChangeUrl("http://testblog1-changed.myblogs.com");
				_blogRepository.Update(blog1);

				_blogDapperRepository.Get(blog1.Id).ShouldNotBeNull();

				uow.Complete();
			}

			//Arrange

			//Assert
			isTriggered.ShouldBeTrue();
		}

		[Fact]
		public async Task should_trigger_event_on_inserted()
		{
			var triggerCount = 0;

            The<IEventBus>().Register<BlogCreatedEvent>(
                @event =>
                {
                    @event.Name.ShouldBe("OnSoftware");
                    triggerCount++;
                });

            using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin())
			{
				_blogRepository.Insert(new Blog("OnSoftware", "www.stove.com"));
				uow.Complete();
			}

			triggerCount.ShouldBe(1);
		}
	}
}
