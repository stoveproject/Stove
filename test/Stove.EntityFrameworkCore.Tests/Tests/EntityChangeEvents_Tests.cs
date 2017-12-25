using System;

using Shouldly;

using Stove.Domain.Repositories;
using Stove.EntityFrameworkCore.Tests.Domain;
using Stove.Events.Bus;
using Stove.Events.Bus.Entities;

using Xunit;

namespace Stove.EntityFrameworkCore.Tests.Tests
{
    public class EntityChangeEvents_Tests : EntityFrameworkCoreTestBase
    {
        private readonly IRepository<Blog> _blogRepository;
        private readonly IEventBus _eventBus;

        public EntityChangeEvents_Tests()
        {
	        Building(builder => { }).Ok();

			_blogRepository = The<IRepository<Blog>>();
            _eventBus = The<IEventBus>();
        }

        [Fact]
        public void Complex_Event_Test()
        {
            string blogName = Guid.NewGuid().ToString("N");

            var creatingEventTriggered = false;
            var createdEventTriggered = false;
            var updatingEventTriggered = false;
            var updatedEventTriggered = false;
            var blogUrlChangedEventTriggered = false;

            _eventBus.Register<EntityCreatingEvent<Blog>>(data =>
            {
                creatingEventTriggered.ShouldBeFalse();
                createdEventTriggered.ShouldBeFalse();
                updatingEventTriggered.ShouldBeFalse();
                updatedEventTriggered.ShouldBeFalse();
                blogUrlChangedEventTriggered.ShouldBeFalse();

                creatingEventTriggered = true;

                data.Entity.IsTransient().ShouldNotBeNull();
                data.Entity.Name.ShouldBe(blogName);

                /* Want to change url from http:// to https:// (ensure to save https url always)
                 * Expect to trigger EntityUpdatingEvent, EntityUpdatedEvent and BlogUrlChangedEvent events */
                data.Entity.Url.ShouldStartWith("http://");
                data.Entity.ChangeUrl(data.Entity.Url.Replace("http://", "https://"));
            });

            _eventBus.Register<EntityCreatedEvent<Blog>>(data =>
            {
                creatingEventTriggered.ShouldBeTrue();
                createdEventTriggered.ShouldBeFalse();
                updatingEventTriggered.ShouldBeTrue();
                updatedEventTriggered.ShouldBeFalse();
                blogUrlChangedEventTriggered.ShouldBeTrue();

                createdEventTriggered = true;

                data.Entity.IsTransient().ShouldNotBeNull();
                data.Entity.Name.ShouldBe(blogName);
            });

            _eventBus.Register<EntityUpdatingEvent<Blog>>(data =>
            {
                creatingEventTriggered.ShouldBeTrue();
                createdEventTriggered.ShouldBeFalse();
                updatingEventTriggered.ShouldBeFalse();
                updatedEventTriggered.ShouldBeFalse();
                blogUrlChangedEventTriggered.ShouldBeFalse();

                updatingEventTriggered = true;

                data.Entity.IsTransient().ShouldNotBeNull();
                data.Entity.Name.ShouldBe(blogName);
                data.Entity.Url.ShouldStartWith("https://");
            });

            _eventBus.Register<EntityUpdatedEvent<Blog>>(data =>
            {
                creatingEventTriggered.ShouldBeTrue();
                createdEventTriggered.ShouldBeTrue();
                updatingEventTriggered.ShouldBeTrue();
                updatedEventTriggered.ShouldBeFalse();
                blogUrlChangedEventTriggered.ShouldBeTrue();

                updatedEventTriggered = true;

                data.Entity.IsTransient().ShouldNotBeNull();
                data.Entity.Name.ShouldBe(blogName);
                data.Entity.Url.ShouldStartWith("https://");
            });

            _eventBus.Register<BlogUrlChangedEvent>(data =>
            {
                creatingEventTriggered.ShouldBeTrue();
                createdEventTriggered.ShouldBeFalse();
                updatingEventTriggered.ShouldBeTrue();
                updatedEventTriggered.ShouldBeFalse();
                blogUrlChangedEventTriggered.ShouldBeFalse();

                blogUrlChangedEventTriggered = true;

                data.Blog.IsTransient().ShouldNotBeNull();
                data.Blog.Name.ShouldBe(blogName);
                data.Blog.Url.ShouldStartWith("https://");
            });

            _blogRepository.Insert(new Blog(blogName, "http://stove.com"));
        }
    }
}
