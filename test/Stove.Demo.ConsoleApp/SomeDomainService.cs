using System.Collections.Generic;
using System.Linq;

using Autofac.Extras.IocManager;

using Stove.BackgroundJobs;
using Stove.Dapper.Dapper.Repositories;
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
        private readonly IDapperRepository<Animal> _animalDapperRepository;
        private readonly IDbContextProvider<AnimalStoveDbContext> _animalDbContextProvider;
        private readonly IRepository<Animal> _animalRepository;
        private readonly IBackgroundJobManager _hangfireBackgroundJobManager;
        private readonly IDapperRepository<Person> _personDapperRepository;
        private readonly IRepository<Person> _personRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public SomeDomainService(
            IRepository<Person> personRepository,
            IRepository<Animal> animalRepository,
            IUnitOfWorkManager unitOfWorkManager,
            IDbContextProvider<AnimalStoveDbContext> animalDbContextProvider,
            IBackgroundJobManager hangfireBackgroundJobManager,
            IDapperRepository<Person> personDapperRepository,
            IDapperRepository<Animal> animalDapperRepository)
        {
            _personRepository = personRepository;
            _animalRepository = animalRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _animalDbContextProvider = animalDbContextProvider;
            _hangfireBackgroundJobManager = hangfireBackgroundJobManager;
            _personDapperRepository = personDapperRepository;
            _animalDapperRepository = animalDapperRepository;
            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }

        public void DoSomeStuff()
        {
            using (IUnitOfWorkCompleteHandle uow = _unitOfWorkManager.Begin())
            {
                //Logger.Debug("Uow Began!");

                //_personRepository.Insert(new Person("Oğuzhan"));
                //_personRepository.Insert(new Person("Ekmek"));

                //_animalRepository.Insert(new Animal("Kuş"));
                //_animalRepository.Insert(new Animal("Kedi"));

                //_animalDbContextProvider.GetDbContext().Animals.Add(new Animal("Kelebek"));

                //_unitOfWorkManager.Current.SaveChanges();

                //Person person = _personRepository.FirstOrDefault(x => x.Name == "Oğuzhan");
                //Animal animal = _animalRepository.FirstOrDefault(x => x.Name == "Kuş");

                var birds = _animalDapperRepository.GetSet(new { Name = "Kuş" }, 0, 10, "Id");

                IEnumerable<Person> personFromDapper = _personDapperRepository.GetList(new { Name = "Oğuzhan" });
                IEnumerable<Person> person2FromDapper = _personDapperRepository.Query("select * from Person with(nolock) where name =@name", new { name = "Oğuzhan" });


                birds = birds.ToList();

                uow.Complete();

              

                //_hangfireBackgroundJobManager.EnqueueAsync<SimpleBackgroundJob, SimpleBackgroundJobArgs>(new SimpleBackgroundJobArgs
                //{
                //    Message = "Oğuzhan"
                //});

                //Logger.Debug("Uow End!");
            }
        }
    }
}
