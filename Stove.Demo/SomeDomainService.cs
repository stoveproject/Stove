using System;

using Autofac.Extras.IocManager;

using Stove.Demo.Entities;
using Stove.Domain.Repositories;
using Stove.Domain.Uow;
using Stove.Log;

namespace Stove.Demo
{
    public class SomeDomainService : ITransientDependency
    {
        private readonly IRepository<Animal> _animalRepository;
        private readonly IRepository<Person> _personRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public SomeDomainService(IRepository<Person> personRepository, IRepository<Animal> animalRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _personRepository = personRepository;
            _animalRepository = animalRepository;
            _unitOfWorkManager = unitOfWorkManager;
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

                _unitOfWorkManager.Current.SaveChanges();

                _personRepository.FirstOrDefault(x => x.Name == "Oğuzhan");
                _animalRepository.FirstOrDefault(x => x.Name == "Kuş");

                uow.Complete();

                Logger.Debug("Uow End!");
            }
        }
    }
}
