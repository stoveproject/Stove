using System;
using System.Data.Entity;
using System.Reflection;

using Autofac.Extras.IocManager;

using Hangfire;

using HibernatingRhinos.Profiler.Appender.EntityFramework;

using Stove.Dapper;
using Stove.Demo.ConsoleApp.DbContexes;
using Stove.EntityFramework;
using Stove.Hangfire;
using Stove.Hangfire.Hangfire;
using Stove.Mapster;
using Stove.NLog;
using Stove.Redis;

namespace Stove.Demo.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            EntityFrameworkProfiler.Initialize();

            Database.SetInitializer(new NullDatabaseInitializer<AnimalStoveDbContext>());
            Database.SetInitializer(new NullDatabaseInitializer<PersonStoveDbContext>());

            IRootResolver resolver = IocBuilder.New
                                               .UseAutofacContainerBuilder()
                                               .UseStove(starterBootstrapperType: typeof(StoveDemoBootstrapper), autoUnitOfWorkInterceptionEnabled: true)
                                               .UseStoveEntityFramework()
                                               .UseStoveDapper()
                                               .UseStoveMapster()
                                               .UseStoveDefaultEventBus()
                                               .UseStoveDbContextEfTransactionStrategy()
                                               .UseStoveTypedConnectionStringResolver()
                                               .UseStoveNLog()
                                               .UseStoveBackgroundJobs()
                                               //.UseStoveMemoryCaching()
                                               .UseStoveRedisCaching()
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

            Console.ReadKey();
        }
    }
}
