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

            IRootResolver resolver = IocBuilder.New
                                               .UseAutofacContainerBuilder()
                                               .UseStove()
                                               .UseEntityFramework()
                                               .UseDefaultEventBus()
                                               .UseDbContextEfTransactionStrategy()
                                               //.UseTransacitonScopeEfTransactionStrategy()
                                               .RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly()))
                                               .UseNLog()
                                               .CreateResolver();

            Database.SetInitializer(new CreateDatabaseIfNotExists<DemoStoveDbContext>());

            var someDomainService = resolver.Resolve<SomeDomainService>();
            someDomainService.DoSomeStuff();
        }
    }
}
