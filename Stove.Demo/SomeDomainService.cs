using Autofac.Extras.IocManager;

using Stove.Demo.Entities;
using Stove.Domain.Repositories;
using Stove.Domain.Uow;

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
        }

        public void DoSomeStuff()
        {
            using (_unitOfWorkManager.Begin())
            {
                _personRepository.Insert(new Person("Oğuzhan"));
                _animalRepository.Insert(new Animal("Kuş"));

                _unitOfWorkManager.Current.SaveChanges();

                _personRepository.FirstOrDefault(x => x.Name == "Oğuzhan");
                _animalRepository.FirstOrDefault(x => x.Name == "Kuş");
            }
        }
    }
}
