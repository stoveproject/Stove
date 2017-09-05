using Microsoft.EntityFrameworkCore;

using Stove.EntityFrameworkCore.Dapper.Tests.Domain;

namespace Stove.EntityFrameworkCore.Dapper.Tests.Ef
{
	public class BloggingDbContext : StoveDbContext
	{
		public BloggingDbContext(DbContextOptions<BloggingDbContext> options)
			: base(options)
		{
		}

		public DbSet<Blog> Blogs { get; set; }

		public DbSet<Post> Posts { get; set; }

		public DbSet<Comment> Comments { get; set; }
	}
}
