using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Shouldly;

using Stove.Domain.Repositories;
using Stove.Domain.Uow;
using Stove.EntityFrameworkCore.Tests.Domain;
using Stove.EntityFrameworkCore.Tests.Domain.Events;
using Stove.EntityFrameworkCore.Tests.Ef;
using Stove.Events.Bus; 

using Xunit;

namespace Stove.EntityFrameworkCore.Tests.Tests
{
    public class Repository_Tests : EntityFrameworkCoreTestBase
    {
        private readonly IRepository<Blog> _blogRepository;
        private readonly IRepository<Post, Guid> _postRepository;
        private readonly IUnitOfWorkManager _uowManager;

        public Repository_Tests()
        {
            Building(builder => { }).Ok();

            _uowManager = The<IUnitOfWorkManager>();
            _blogRepository = The<IRepository<Blog>>();
            _postRepository = The<IRepository<Post, Guid>>();
        }

        [Fact]
        public void Should_Get_Initial_Blogs()
        {
            //Act

            List<Blog> blogs = _blogRepository.GetAllList();

            //Assert

            blogs.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task Should_Automatically_Save_Changes_On_Uow()
        {
            int blog1Id;

            //Act

            using (IUnitOfWorkCompleteHandle uow = _uowManager.Begin())
            {
                Blog blog1 = await _blogRepository.SingleAsync(b => b.Name == "test-blog-1");
                blog1Id = blog1.Id;

                blog1.Name = "test-blog-1-updated";

                await uow.CompleteAsync();
            }

            //Assert

            await UsingDbContextAsync<BloggingDbContext>(async context =>
            {
                Blog blog1 = await context.Blogs.SingleAsync(b => b.Id == blog1Id);
                blog1.Name.ShouldBe("test-blog-1-updated");
            });
        }

        [Fact]
        public async Task Should_Include_Navigation_Properties_Because_It_Is_Lazy_Loaded()
        {
            using (IUnitOfWorkCompleteHandle uow = _uowManager.Begin())
            {
                Post post = await _postRepository.GetAll().FirstAsync();

                post.Blog.ShouldNotBeNull();

                await uow.CompleteAsync();
            }
        }

        [Fact]
        public async Task Should_Include_Navigation_Properties_If_Requested()
        {
            using (IUnitOfWorkCompleteHandle uow = _uowManager.Begin())
            {
                Post post = await _postRepository.GetAllIncluding(p => p.Blog).FirstAsync();

                post.Blog.ShouldNotBeNull();
                post.Blog.Name.ShouldBe("test-blog-1");

                await uow.CompleteAsync();
            }
        }

        [Fact]
        public async Task Should_Insert_New_Entity()
        {
            using (IUnitOfWorkCompleteHandle uow = _uowManager.Begin())
            {
                var blog = new Blog("blog2", "http://myblog2.com");
                blog.IsTransient().ShouldBeTrue();
                await _blogRepository.InsertAsync(blog);
                await uow.CompleteAsync();
                blog.IsTransient().ShouldBeFalse();
            }
        }

        [Fact]
        public async Task Should_Insert_New_Entity_With_Guid_Id()
        {
            using (IUnitOfWorkCompleteHandle uow = _uowManager.Begin())
            {
                Blog blog1 = await _blogRepository.GetAsync(1);
                var post = new Post(blog1, "a test title", "a test body");
                post.IsTransient().ShouldBeTrue();
                await _postRepository.InsertAsync(post);
                await uow.CompleteAsync();
                post.IsTransient().ShouldBeFalse();
            }
        }

        [Fact]
        public async Task should_rolled_back_when_CancellationToken_is_requested_as_cancel()
        {
            var ts = new CancellationTokenSource();

            The<IEventBus>().Register<BlogCreatedEvent>((@event, headers) => 
            {
                ts.Cancel(true);
            });

            try
            {
                using (IUnitOfWorkCompleteHandle uow = _uowManager.Begin())
                {
                    await _blogRepository.InsertAsync(new Blog("cancellationtoken", "cancellationtoken.com"));

                    _blogRepository.FirstOrDefaultAsync(x => x.Name == "cancellationtoken").ShouldNotBeNull();

                    await uow.CompleteAsync(ts.Token);
                }
            }
            catch (Exception exception)
            {
                //Handle uow should be Rolled Back!
            }

            using (IUnitOfWorkCompleteHandle uow = _uowManager.Begin())
            {
                Blog blog = await _blogRepository.FirstOrDefaultAsync(x => x.Name == "cancellationtoken");

                blog.ShouldBeNull();

                await uow.CompleteAsync();
            }
        }
    }
}
