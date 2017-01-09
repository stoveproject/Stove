using System;
using System.Data.Entity;
using System.Reflection;

using Autofac.Extras.IocManager;

using Hangfire;

using HibernatingRhinos.Profiler.Appender.EntityFramework;

using Stove.Demo.ConsoleApp.DbContexes;
using Stove.EntityFramework;
using Stove.Hangfire.Hangfire;
using Stove.NLog;

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
                                               .UseStove(autoUnitOfWorkInterceptionEnabled: true)
                                               .UseStoveEntityFramework()
                                               .UseDefaultEventBus()
                                               .UseDbContextEfTransactionStrategy()
                                               .UseTypedConnectionStringResolver()
                                               .UseNLog()
                                               .UseBackgroundJobs()
                                               .UseHangfire(configuration =>
                                               {
                                                   configuration.GlobalConfiguration.UseSqlServerStorage("Default")
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
