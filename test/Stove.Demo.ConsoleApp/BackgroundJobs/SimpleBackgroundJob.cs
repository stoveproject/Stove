using Autofac.Extras.IocManager;

using Stove.BackgroundJobs;
using Stove.Demo.ConsoleApp.Entities;
using Stove.Domain.Repositories;
using Stove.Domain.Uow;

namespace Stove.Demo.ConsoleApp.BackgroundJobs
{
    public class SimpleBackgroundJob : BackgroundJob<SimpleBackgroundJobArgs>, ITransientDependency
    {
        private readonly IRepository<Person> _personRepository;

        public SimpleBackgroundJob(IRepository<Person> personRepository)
        {
            _personRepository = personRepository;
        }

        public override void Execute(SimpleBackgroundJobArgs args)
        {
            using (IUnitOfWorkCompleteHandle uow = UnitOfWorkManager.Begin())
            {
                string message = _personRepository.FirstOrDefault(person => person.Name == args.Message).Name;
                uow.Complete();
            }
        }
    }
}
