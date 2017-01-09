using Autofac.Extras.IocManager;

using Stove.BackgroundJobs;
using Stove.Demo.ConsoleApp.BackgroundJobs;
using Stove.Demo.ConsoleApp.DbContexes;
using Stove.Demo.ConsoleApp.Entities;
using Stove.Domain.Repositories;
using Stove.Domain.Uow;
using Stove.EntityFramework.EntityFramework;
using Stove.Log;

namespace Stove.Demo.ConsoleApp
{
    public class SomeDomainService : ITransientDependency
    {
        private readonly IDbContextProvider<AnimalStoveDbContext> _animalDbContextProvider;
        private readonly IRepository<Animal> _animalRepository;
        private readonly IBackgroundJobManager _hangfireBackgroundJobManager;
        private readonly IRepository<Person> _personRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public SomeDomainService(
            IRepository<Person> personRepository,
            IRepository<Animal> animalRepository,
            IUnitOfWorkManager unitOfWorkManager,
            IDbContextProvider<AnimalStoveDbContext> animalDbContextProvider,
            IBackgroundJobManager hangfireBackgroundJobManager)
        {
            _personRepository = personRepository;
            _animalRepository = animalRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _animalDbContextProvider = animalDbContextProvider;
            _hangfireBackgroundJobManager = hangfireBackgroundJobManager;
            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }

        public void DoSomeStuff()
        {
            using (IUnitOfWorkCompleteHandle uow = _unitOfWorkManager.Begin())
            {
                Logger.Debug("Uow Began!");

                _personRepository.Insert(new Person("Oğuzhan"));
                _personRepository.Insert(new Person("Ekmek"));

                _animalRepository.Insert(new Animal("Kuş"));
                _animalRepository.Insert(new Animal("Kedi"));

                _animalDbContextProvider.GetDbContext().Animals.Add(new Animal("Kelebek"));

                _unitOfWorkManager.Current.SaveChanges();

                Person person = _personRepository.FirstOrDefault(x => x.Name == "Oğuzhan");
                Animal animal = _animalRepository.FirstOrDefault(x => x.Name == "Kuş");

                uow.Complete();

                _hangfireBackgroundJobManager.EnqueueAsync<SimpleBackgroundJob, SimpleBackgroundJobArgs>(new SimpleBackgroundJobArgs
                {
                    Message = "Oğuzhan"
                });

                Logger.Debug("Uow End!");
            }
        }
    }
}
