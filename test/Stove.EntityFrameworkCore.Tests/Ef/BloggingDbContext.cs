using Microsoft.EntityFrameworkCore;

using Stove.EntityFrameworkCore.Tests.Domain;

namespace Stove.EntityFrameworkCore.Tests.Ef
{
    public class BloggingDbContext : StoveDbContext
    {
        public DbSet<Blog> Blogs { get; set; }

        public DbSet<Post> Posts { get; set; }

        public BloggingDbContext(DbContextOptions<BloggingDbContext> options)
            : base(options)
        {
            
        }
    }
}
