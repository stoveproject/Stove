using Microsoft.EntityFrameworkCore;

using Stove.Demo.WebApi.Core.Domain.Entities;
using Stove.EntityFrameworkCore;

namespace Stove.Demo.WebApi.Core.Domain.DbContexts
{
	public class AnimalDbContext : StoveDbContext
	{
		public AnimalDbContext(DbContextOptions<AnimalDbContext> options) : base(options)
		{
		}

		public virtual DbSet<Animal> Animals { get; set; }
	}
}
