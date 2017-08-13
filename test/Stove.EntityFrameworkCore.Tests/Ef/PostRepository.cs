using System;

using Stove.EntityFrameworkCore.Repositories;
using Stove.EntityFrameworkCore.Tests.Domain;

namespace Stove.EntityFrameworkCore.Tests.Ef
{
    public class PostRepository : EfCoreRepositoryBase<BloggingDbContext, Post, Guid>, IPostRepository
    {
        public PostRepository(IDbContextProvider<BloggingDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public override int Count()
        {
            throw new Exception("can not get count of posts");
        }
    }
}
