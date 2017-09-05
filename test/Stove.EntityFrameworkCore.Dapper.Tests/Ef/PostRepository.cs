using System;

using Stove.EntityFrameworkCore.Dapper.Tests.Domain;
using Stove.EntityFrameworkCore.Repositories;

namespace Stove.EntityFrameworkCore.Dapper.Tests.Ef
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
