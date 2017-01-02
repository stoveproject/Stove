using System.Data.Entity;
using System.Reflection;

using Autofac.Extras.IocManager;

using HibernatingRhinos.Profiler.Appender.EntityFramework;

using Stove.Demo.DbContexes;
using Stove.EntityFramework;
using Stove.Log;

namespace Stove.Demo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            EntityFrameworkProfiler.Initialize();

            //Database.SetInitializer(new CreateDatabaseIfNotExists<AnimalStoveDbContext>());

            IRootResolver resolver = IocBuilder.New
                                               .UseAutofacContainerBuilder()
                                               .UseStove(configuration =>
                                               {
                                                   configuration.DefaultNameOrConnectionString = "Default";
                                                   return configuration;
                                               })
                                               .UseEntityFramework()
                                               .UseDefaultEventBus()
                                               .UseDbContextEfTransactionStrategy()
                                               .RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly()))
                                               .UseNLog()
                                               .CreateResolver();

            var someDomainService = resolver.Resolve<SomeDomainService>();
            someDomainService.DoSomeStuff();
        }
    }
}
