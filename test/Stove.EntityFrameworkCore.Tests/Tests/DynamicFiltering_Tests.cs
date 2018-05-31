using Microsoft.EntityFrameworkCore;
using Shouldly;
using Stove.Domain.Repositories;
using Stove.Domain.Uow;
using Stove.EntityFrameworkCore.Tests.Domain;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Stove.EntityFrameworkCore.Tests.Tests
{
    public class DynamicFiltering_Tests : EntityFrameworkCoreTestBase
    {
        private readonly IRepository<Blog> _blogRepository;

        public DynamicFiltering_Tests()
        {
            Building(builder => { }).Ok();

            _blogRepository = The<IRepository<Blog>>();
        }

        [Fact]
        public async Task Should_Filter_Soft_Deleted_Record()
        {
            using (var uow = The<IUnitOfWorkManager>().Begin())
            {
                var blog = await _blogRepository.FirstOrDefaultAsync(b => b.Name == "test-blog-1");

                await _blogRepository.DeleteAsync(blog);

                await uow.CompleteAsync();
            }

            using (var uow = The<IUnitOfWorkManager>().Begin())
            {
                var blog = await _blogRepository.FirstOrDefaultAsync(b => b.Name == "test-blog-1");

                blog.ShouldBeNull();
            }
        }

        [Fact]
        public async Task Should_Filter_Soft_Deleted_For_Lazy_Loaded_Child_Collection()
        {
            using (var uow = The<IUnitOfWorkManager>().Begin())
            {
                var blog = await _blogRepository.FirstOrDefaultAsync(b => b.Name == "test-blog-1");

                blog.Posts.Count.ShouldBe(2);

                blog.Posts.Remove(blog.Posts.First());

                await uow.CompleteAsync();
            }

            using (var uow = The<IUnitOfWorkManager>().Begin())
            {
                var blog = await _blogRepository.FirstOrDefaultAsync(b => b.Name == "test-blog-1");

                blog.Posts.Count.ShouldBe(1);
            }
        }

        [Fact]
        public async Task Should_Filter_Soft_Deleted_For_Eager_Loaded_Child_Collection()
        {
            using (var uow = The<IUnitOfWorkManager>().Begin())
            {
                var blog = await _blogRepository.GetAll().Where(b => b.Name == "test-blog-1").Include(b => b.Posts).FirstOrDefaultAsync();

                blog.Posts.Count.ShouldBe(2);

                blog.Posts.Remove(blog.Posts.First());

                await uow.CompleteAsync();
            }

            using (var uow = The<IUnitOfWorkManager>().Begin())
            {
                var blog = await _blogRepository.FirstOrDefaultAsync(b => b.Name == "test-blog-1");

                blog.Posts.Count.ShouldBe(1);
            }
        }
    }
}
