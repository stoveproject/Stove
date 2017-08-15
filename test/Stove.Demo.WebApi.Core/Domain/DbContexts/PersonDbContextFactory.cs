//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Design;
//using Microsoft.EntityFrameworkCore.Infrastructure;
//using Microsoft.Extensions.Configuration;

//namespace Stove.Demo.WebApi.Core.Domain.DbContexts
//{
//	public class PersonStoveDbContextFactory : IDesignTimeDbContextFactory<PersonDbContext>
//	{
//		public PersonDbContext Create(DbContextFactoryOptions options)
//		{
//			IConfigurationBuilder builder = new ConfigurationBuilder()
//				.SetBasePath(options.ContentRootPath)
//				.AddJsonFile("appsettings.json", true, true)
//				.AddEnvironmentVariables();

//			IConfigurationRoot configuration = builder.Build();

//			DbContextOptions<PersonDbContext> opts = new DbContextOptionsBuilder<PersonDbContext>()
//				.UseSqlServer(configuration.GetConnectionString("Default"))
//				.Options;

//			return new PersonDbContext(opts);
//		}
//	}
//}
