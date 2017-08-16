using System;
using System.Collections.Generic;
using System.Linq;

using Autofac.Extras.IocManager;

using MassTransit;

using Stove.BackgroundJobs;
using Stove.Dapper.Repositories;
using Stove.Demo.WebApi.Core.Application.Dto;
using Stove.Demo.WebApi.Core.Application.RabbitMQ.Messages;
using Stove.Demo.WebApi.Core.Domain.DbContexts;
using Stove.Demo.WebApi.Core.Domain.Entities;
using Stove.Domain.Repositories;
using Stove.Domain.Uow;
using Stove.EntityFrameworkCore;
using Stove.Log;
using Stove.Mapster;
using Stove.MQ;
using Stove.Runtime.Caching;
using Stove.Runtime.Session;

namespace Stove.Demo.WebApi.Core.Domain
{
	public class SomeDomainService : ITransientDependency
	{
		private readonly IDapperRepository<Animal> _animalDapperRepository;
		private readonly IDbContextProvider<AnimalDbContext> _animalDbContextProvider;
		private readonly IRepository<Animal> _animalRepository;
		private readonly ICacheManager _cacheManager;
		private readonly IBackgroundJobManager _hangfireBackgroundJobManager;
		private readonly IScheduleJobManager _hangfireScheduleJobManager;
		private readonly IDapperRepository<Mail, Guid> _mailDapperRepository;
		private readonly IMessageBus _messageBus;
		private readonly IDapperRepository<Person> _personDapperRepository;
		private readonly IRepository<Person> _personRepository;
		private readonly IDapperRepository<Product> _productDapperRepository;
		private readonly IUnitOfWorkManager _unitOfWorkManager;

		public SomeDomainService(
			IRepository<Person> personRepository,
			IRepository<Animal> animalRepository,
			IUnitOfWorkManager unitOfWorkManager,
			IDbContextProvider<AnimalDbContext> animalDbContextProvider,
			IBackgroundJobManager hangfireBackgroundJobManager,
			IDapperRepository<Person> personDapperRepository,
			IDapperRepository<Animal> animalDapperRepository,
			ICacheManager cacheManager,
			IMessageBus messageBus,
			IScheduleJobManager hangfireScheduleJobManager,
			IDapperRepository<Product> productDapperRepository,
			IDapperRepository<Mail, Guid> mailDapperRepository)
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
			_hangfireScheduleJobManager = hangfireScheduleJobManager;
			_productDapperRepository = productDapperRepository;
			_mailDapperRepository = mailDapperRepository;

			Logger = NullLogger.Instance;
		}

		public IStoveSession StoveSession { get; set; }

		public ILogger Logger { get; set; }

		public void DoSomeStuff()
		{
			try
			{
				using (IUnitOfWorkCompleteHandle uow = _unitOfWorkManager.Begin())
				{
					Logger.Debug("Uow Began!");

					int persionId1 = _personRepository.InsertAndGetId(new Person("Oğuzhan"));
					_personRepository.Insert(new Person("Ekmek"));

					int animalId1 = _animalRepository.InsertAndGetId(new Animal("Kuş"));
					_animalRepository.Insert(new Animal("Kedi"));

					_animalDbContextProvider.GetDbContext().Animals.Add(new Animal("Kelebek"));

					_unitOfWorkManager.Current.SaveChanges();

					Person personCache = _cacheManager.GetCache(DemoCacheName.Demo).Get("person", () => _personRepository.FirstOrDefault(x => x.Name == "Oğuzhan"));

					Person person = _personRepository.FirstOrDefault(x => x.Name == "Oğuzhan");
					Animal animal = _animalRepository.FirstOrDefault(x => x.Name == "Kuş");

					using (StoveSession.Use(266))
					{
						_productDapperRepository.Insert(new Product("TShirt1"));
						int gomlekId = _productDapperRepository.InsertAndGetId(new Product("Gomlek1"));

						Product firstProduct = _productDapperRepository.FirstOrDefault(x => x.Name == "TShirt1");
						IEnumerable<Product> products = _productDapperRepository.GetAll();

						firstProduct.Name = "Something";

						_productDapperRepository.Update(firstProduct);

						_mailDapperRepository.Insert(new Mail("New Product Added"));
						Guid mailId = _mailDapperRepository.InsertAndGetId(new Mail("Second Product Added"));

						IEnumerable<Mail> mails = _mailDapperRepository.GetAll();

						Mail firstMail = mails.First();

						firstMail.Subject = "Sorry wrong email!";

						_mailDapperRepository.Update(firstMail);
					}

					Animal oneAnimal = _animalDapperRepository.Get(animalId1);
					Animal oneAnimalAsync = _animalDapperRepository.GetAsync(animalId1).Result;

					Person onePerson = _personDapperRepository.Get(persionId1);
					Person onePersonAsync = _personDapperRepository.GetAsync(persionId1).Result;

					IEnumerable<Animal> birdsSet = _animalDapperRepository.GetSet(x => x.Name == "Kuş", 0, 10, "Id");

					using (_unitOfWorkManager.Current.DisableFilter(StoveDataFilters.SoftDelete))
					{
						IEnumerable<Person> personFromDapperNotFiltered = _personDapperRepository.GetAll(x => x.Name == "Oğuzhan");
					}

					IEnumerable<Person> personFromDapperFiltered = _personDapperRepository.GetAll(x => x.Name == "Oğuzhan");

					IEnumerable<Animal> birdsFromExpression = _animalDapperRepository.GetSet(x => x.Name == "Kuş", 0, 10, "Id");

					IEnumerable<Animal> birdsPagedFromExpression = _animalDapperRepository.GetAllPaged(x => x.Name == "Kuş", 0, 10, "Name");

					IEnumerable<Person> personFromDapperExpression = _personDapperRepository.GetAll(x => x.Name.Contains("Oğuzhan"));

					int birdCount = _animalDapperRepository.Count(x => x.Name == "Kuş");

					var personAnimal = _animalDapperRepository.Query<PersonAnimal>("select Name as PersonName,'Zürafa' as AnimalName from Persons with(nolock) where name=@name", new { name = "Oğuzhan" })
															  .MapTo<List<PersonAnimalDto>>();

					birdsFromExpression.ToList();
					birdsPagedFromExpression.ToList();
					birdsSet.ToList();

					IEnumerable<Person> person2FromDapper = _personDapperRepository.Query("select * from Persons with(nolock) where name =@name", new { name = "Oğuzhan" });

					_personDapperRepository.Insert(new Person("oğuzhan2"));
					int id = _personDapperRepository.InsertAndGetId(new Person("oğuzhan3"));
					Person person3 = _personDapperRepository.Get(id);
					person3.Name = "oğuzhan4";
					_personDapperRepository.Update(person3);
					_personDapperRepository.Delete(person3);

					Person person2Cache = _cacheManager.GetCache(DemoCacheName.Demo).Get("person", () => _personRepository.FirstOrDefault(x => x.Name == "Oğuzhan"));

					//Person oguzhan = _personRepository.Nolocking(persons => persons.FirstOrDefault(x => x.Name == "Oğuzhan"));

					Person oguzhan2 = _personRepository.FirstOrDefault(x => x.Name == "Oğuzhan");

					uow.Complete();

					_messageBus.Publish<IPersonAddedMessage>(new PersonAddedMessage
					{
						Name = "Oğuzhan",
						CorrelationId = NewId.NextGuid()
					});

					//_hangfireBackgroundJobManager.EnqueueAsync<SimpleBackgroundJob, SimpleBackgroundJobArgs>(new SimpleBackgroundJobArgs
					//{
					//    Message = "Oğuzhan"
					//});

					//_hangfireScheduleJobManager.ScheduleAsync<SimpleBackgroundJob, SimpleBackgroundJobArgs>(new SimpleBackgroundJobArgs
					//{
					//    Message = "Oğuzhan"
					//}, Cron.Minutely());

					Logger.Debug("Uow End!");
				}
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
				throw ex;
			}

		}
	}
}
