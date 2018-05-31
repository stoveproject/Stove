using System;
using System.Threading.Tasks;

using Shouldly;

using Stove.Domain.Repositories;
using Stove.Domain.Uow;
using Stove.EntityFrameworkCore.Tests.Domain;

using Xunit;

namespace Stove.EntityFrameworkCore.Tests.Tests
{
	public class LazyLoading_Tests : EntityFrameworkCoreTestBase
	{
		private readonly IRepository<Blog> _blogRepository;
		private readonly IRepository<Post, Guid> _postRepository;

		public LazyLoading_Tests()
		{
			Building(builder => { }).Ok();

			_blogRepository = The<IRepository<Blog>>();
			_postRepository = The<IRepository<Post, Guid>>();
		}

		[Fact]
		public async Task Should_Lazy_Load_Collections()
		{
			using (var uow = The<IUnitOfWorkManager>().Begin())
			{
				var blog = await _blogRepository.FirstOrDefaultAsync(b => b.Name == "test-blog-1");

				blog.ShouldNotBeNull();
				blog.Posts.ShouldNotBeNull();
				blog.Posts.Count.ShouldBeGreaterThan(0);

				await uow.CompleteAsync();
			}
		}

		[Fact]
		public async Task Should_Lazy_Load_Properties()
		{
			using (var uow = The<IUnitOfWorkManager>().Begin())
			{
				var post = await _postRepository.FirstOrDefaultAsync(b => b.Title == "test-post-1-title");
				post.ShouldNotBeNull();
				post.Blog.ShouldNotBeNull();
				post.Blog.Name.ShouldBe("test-blog-1");

				await uow.CompleteAsync();
			}
		}
	}
}
