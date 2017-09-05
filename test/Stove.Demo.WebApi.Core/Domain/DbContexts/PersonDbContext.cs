using Microsoft.EntityFrameworkCore;

using Stove.Demo.WebApi.Core.Domain.Entities;
using Stove.EntityFrameworkCore;

namespace Stove.Demo.WebApi.Core.Domain.DbContexts
{
	public class PersonDbContext : StoveDbContext
	{
		public PersonDbContext(DbContextOptions<PersonDbContext> options) : base(options)
		{
		}

		public virtual DbSet<Person> Persons { get; set; }

		public virtual DbSet<Product> Products { get; set; }

		public virtual DbSet<Mail> Mails { get; set; }
	}
}
