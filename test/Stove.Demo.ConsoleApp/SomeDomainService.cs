using System.Collections.Generic;
using System.Linq;

using Autofac.Extras.IocManager;

using LinqKit;

using MassTransit;

using Stove.BackgroundJobs;
using Stove.Dapper.Dapper.Repositories;
using Stove.Demo.ConsoleApp.BackgroundJobs;
using Stove.Demo.ConsoleApp.DbContexes;
using Stove.Demo.ConsoleApp.Dto;
using Stove.Demo.ConsoleApp.Entities;
using Stove.Demo.ConsoleApp.RabbitMQ.Messages;
using Stove.Domain.Repositories;
using Stove.Domain.Uow;
using Stove.EntityFramework.EntityFramework;
using Stove.EntityFramework.EntityFramework.Extensions;
using Stove.Log;
using Stove.Mapster.Mapster;
using Stove.MQ;
using Stove.Runtime.Caching;

namespace Stove.Demo.ConsoleApp
{
    public class SomeDomainService : ITransientDependency
    {
        private readonly IDapperRepository<Animal> _animalDapperRepository;
        private readonly IDbContextProvider<AnimalStoveDbContext> _animalDbContextProvider;
        private readonly IRepository<Animal> _animalRepository;
        private readonly ICacheManager _cacheManager;
        private readonly IBackgroundJobManager _hangfireBackgroundJobManager;
        private readonly IMessageBus _messageBus;
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
            IDapperRepository<Animal> animalDapperRepository,
            ICacheManager cacheManager,
            IMessageBus messageBus)
        {
            _personRepository = personRepository;
            _animalRepository = animalRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _animalDbContextProvider = animalDbContextProvider;
            _hangfireBackgroundJobManager = hangfireBackgroundJobManager;
            _personDapperRepository = personDapperRepository;
            _animalDapperRepository = animalDapperRepository;
            _cacheManager = cacheManager;
            _messageBus = messageBus;
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

                Person personCache = _cacheManager.GetCache(DemoCacheName.Demo).Get("person", () => _personRepository.FirstOrDefault(x => x.Name == "Oğuzhan"));

                Person person = _personRepository.FirstOrDefault(x => x.Name == "Oğuzhan");
                Animal animal = _animalRepository.FirstOrDefault(x => x.Name == "Kuş");

                #region DAPPER

                var list = new List<string>
                {
                    "elma", "armut"
                };

                ExpressionStarter<Animal> predicate = PredicateBuilder.New<Animal>();
                predicate.And(x => x.Name == "Kuş");

                IEnumerable<Animal> birdsSet = _animalDapperRepository.GetSet(new { Name = "Kuş" }, 0, 10, "Id");

                IEnumerable<Person> personFromDapper = _personDapperRepository.GetList(new { Name = "Oğuzhan" });

                IEnumerable<Animal> birdsFromExpression = _animalDapperRepository.GetSet(predicate, 0, 10, "Id");

                IEnumerable<Animal> birdsPagedFromExpression = _animalDapperRepository.GetListPaged(x => x.Name == "Kuş", 0, 10, "Name");

                IEnumerable<Person> personFromDapperExpression = _personDapperRepository.GetList(x => x.Name.Contains("Oğuzhan"));

                int birdCount = _animalDapperRepository.Count(x => x.Name == "Kuş");

                var personAnimal = _animalDapperRepository.Query<PersonAnimal>("select Name as PersonName,'Zürafa' as AnimalName from Person with(nolock) where name=@name", new { name = "Oğuzhan" })
                                                          .MapTo<List<PersonAnimalDto>>();

                birdsFromExpression.ToList();
                birdsPagedFromExpression.ToList();
                birdsSet.ToList();

                IEnumerable<Person> person2FromDapper = _personDapperRepository.Query("select * from Person with(nolock) where name =@name", new { name = "Oğuzhan" });

                _personDapperRepository.Insert(new Person("oğuzhan2"));
                int id = _personDapperRepository.InsertAndGetId(new Person("oğuzhan3"));
                Person person3 = _personDapperRepository.Get(id);
                person3.Name = "oğuzhan4";
                _personDapperRepository.Update(person3);
                _personDapperRepository.Delete(person3);

                #endregion

                Person person2Cache = _cacheManager.GetCache(DemoCacheName.Demo).Get("person", () => _personRepository.FirstOrDefault(x => x.Name == "Oğuzhan"));

                Person oguzhan = _personRepository.Nolocking(persons => persons.FirstOrDefault(x => x.Name == "Oğuzhan"));

                Person oguzhan2 = _personRepository.FirstOrDefault(x => x.Name == "Oğuzhan");

                uow.Complete();

                _messageBus.Publish<IPersonAddedMessage>(new PersonAddedMessage
                {
                    Name = "Oğuzhan",
                    CorrelationId = NewId.NextGuid()
                });

                _hangfireBackgroundJobManager.EnqueueAsync<SimpleBackgroundJob, SimpleBackgroundJobArgs>(new SimpleBackgroundJobArgs
                {
                    Message = "Oğuzhan"
                });

                Logger.Debug("Uow End!");
            }
        }
    }
}
