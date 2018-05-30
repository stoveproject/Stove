using System;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Shouldly;

using Stove.Domain.Repositories;
using Stove.Domain.Uow;
using Stove.EntityFrameworkCore.Tests.Domain;
using Stove.EntityFrameworkCore.Tests.Ef;
using Xunit;

namespace Stove.EntityFrameworkCore.Tests.Tests
{
    //WE CAN NOT TEST TRANSACTIONS SINCE INMEMORY DB DOES NOT SUPPORT IT! TODO: Use SQLite
    public class Transaction_Tests : EntityFrameworkCoreTestBase
    {
        private readonly IUnitOfWorkManager _uowManager;
        private readonly IRepository<Blog> _blogRepository;

        public Transaction_Tests()
        {
	        Building(builder => { }).Ok();

			_uowManager = The<IUnitOfWorkManager>();
            _blogRepository = The<IRepository<Blog>>();
        }

        //[Fact] 
        public async Task Should_Rollback_Transaction_On_Failure()
        {
            const string exceptionMessage = "This is a test exception!";

            var blogName = Guid.NewGuid().ToString("N");

            try
            {
                using (_uowManager.Begin())
                {
                    await _blogRepository.InsertAsync(
                        new Blog(blogName, $"http://{blogName}.com/")
                        );

                    throw new Exception(exceptionMessage);
                }
            }
            catch (Exception ex) when (ex.Message == exceptionMessage)
            {

            }
            
            await UsingDbContextAsync<BloggingDbContext>(async context =>
            {
                var blog = await context.Blogs.FirstOrDefaultAsync(b => b.Name == blogName);
                blog.ShouldNotBeNull();
            });
        }
    }
}