using System.Data.Entity;
using System.Reflection;

using Autofac.Extras.IocManager;

using HibernatingRhinos.Profiler.Appender.EntityFramework;

using Stove.Domain.Repositories;
using Stove.Domain.Uow;
using Stove.EntityFramework;

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
                                               .RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly()))
                                               .CreateResolver();

            Database.SetInitializer(new CreateDatabaseIfNotExists<DemoStoveDbContext>());

            var uowManager = resolver.Resolve<IUnitOfWorkManager>();

            using (IUnitOfWorkCompleteHandle uow = uowManager.Begin())
            {
                var personRepository = resolver.Resolve<IRepository<Person>>();

                personRepository.Insert(new Person { Id = 4, Name = "Oğuzhan" });

                uowManager.Current.SaveChanges();

                Person person = personRepository.Get(1);

                uow.Complete();
            }
        }
    }
}
