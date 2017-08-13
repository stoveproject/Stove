using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace Stove.Demo.WebApi.Core.Domain.DbContexts
{
	public class AnimalDbContextFactory : IDbContextFactory<AnimalDbContext>
	{
		public AnimalDbContext Create(DbContextFactoryOptions options)
		{
			IConfigurationBuilder builder = new ConfigurationBuilder()
				.SetBasePath(options.ContentRootPath)
				.AddJsonFile("appsettings.json", true, true)
				.AddEnvironmentVariables();

			IConfigurationRoot configuration = builder.Build();

			DbContextOptions<AnimalDbContext> opts = new DbContextOptionsBuilder<AnimalDbContext>()
				.UseSqlServer(configuration.GetConnectionString("Default"))
				.Options;

			return new AnimalDbContext(opts);
		}
	}
}
