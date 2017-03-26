using System;
using System.Data.Entity;
using System.Reflection;

using Autofac.Extras.IocManager;

using Hangfire;

using HibernatingRhinos.Profiler.Appender.EntityFramework;

using Stove.Demo.ConsoleApp.DbContexes;

namespace Stove.Demo.ConsoleApp
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            EntityFrameworkProfiler.Initialize();

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
                                               .UseStoveRedisCaching()
                                               .UseStoveRabbitMQ(configuration =>
                                               {
                                                   configuration.HostAddress = "rabbitmq://localhost/";
                                                   configuration.Username = "admin";
                                                   configuration.Password = "admin";
                                                   configuration.QueueName = "Default";
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
           

            Console.ReadKey();
        }
    }
}
