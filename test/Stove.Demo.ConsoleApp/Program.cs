using System;
using System.Data.Entity;
using System.Reflection;

using Autofac.Extras.IocManager;

using Hangfire;

using HibernatingRhinos.Profiler.Appender.EntityFramework;

using MassTransit;

using Stove.Demo.ConsoleApp.DbContexes;
using Stove.EntityFramework;
using Stove.Redis.Configurations;

namespace Stove.Demo.ConsoleApp
{
    public static class Program
    {
        public static void Main(string[] args)
        {
#if DEBUG
            //EntityFrameworkProfiler.Initialize();
#endif

            Database.SetInitializer(new NullDatabaseInitializer<AnimalStoveDbContext>());
            Database.SetInitializer(new NullDatabaseInitializer<PersonStoveDbContext>());

            IRootResolver resolver = IocBuilder.New
                                               .UseAutofacContainerBuilder()
                                               .UseStove<StoveDemoBootstrapper>(false)
                                               .UseStoveEntityFramework()
                                               .UseStoveDapper()
                                               .UseStoveMapster()
                                               .UseStoveEventBus()
                                               .UseStoveDbContextEfTransactionStrategy()
                                               .UseStoveTypedConnectionStringResolver()
                                               .UseStoveNLog()
                                               .UseStoveBackgroundJobs()
                                               .UseStoveRedisCaching(configuration =>
                                               {
	                                                configuration.ConfigurationOptions
	                                                            .AddEndpoint("127.0.0.1")
	                                                            .SetDefaultDabase(0)
																.SetConnectionTimeOut(TimeSpan.FromMinutes(5));

	                                               return configuration;
                                               })
											   .UseStoveRabbitMQ(configuration =>
											   {
												   configuration.HostAddress = "rabbitmq://localhost/";
												   configuration.Username = "admin";
												   configuration.Password = "admin";
												   configuration.QueueName = "NotCore";
												   return configuration;
											   })
											   .UseStoveHangfire(configuration =>
											   {
												   configuration.GlobalConfiguration
																.UseSqlServerStorage("Default")
																.UseNLogLogProvider();
												   return configuration;
											   })
											   .RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly()))
                                               .CreateResolver();

			var someDomainService = resolver.Resolve<SomeDomainService>();
			someDomainService.DoSomeStuff();

			//var productDomainService = resolver.Resolve<ProductDomainService>();
			//productDomainService.DoSomeStuff();

			//var priceDomainService = resolver.Resolve<PriceDomainService>();
			//priceDomainService.DoSomeStuff();

			resolver.Dispose();

#if DEBUG
           // EntityFrameworkProfiler.Shutdown();
#endif

            Console.ReadKey();
        }
    }
}
