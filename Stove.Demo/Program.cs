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

            Database.SetInitializer(new NullDatabaseInitializer<AnimalStoveDbContext>());
            Database.SetInitializer(new NullDatabaseInitializer<PersonStoveDbContext>());

            IRootResolver resolver = IocBuilder.New
                                               .UseAutofacContainerBuilder()
                                               .UseStove()
                                               .UseStoveEntityFramework()
                                               .UseDefaultEventBus()
                                               .UseDbContextEfTransactionStrategy()
                                               //.UseTransacitonScopeEfTransactionStrategy()
                                               .UseTypedConnectionStringResolver()
                                               .UseNLog()
                                               .RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly()))
                                               .CreateResolver();
            var a = new AnimalStoveDbContext();
            var someDomainService = resolver.Resolve<SomeDomainService>();
            someDomainService.DoSomeStuff();
        }
    }
}
