using Stove.Domain.Repositories;
using Stove.Domain.Services;
using Stove.Domain.Uow;

namespace Stove.Demo.ConsoleApp.RavenDB
{
    public class RavenDomainService : DomainService
    {
        private readonly IRepository<Product> _productRepository;

        public RavenDomainService(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public void DoSomeStuff()
        {
            using (IUnitOfWorkCompleteHandle uow = UnitOfWorkManager.Begin())
            {
                Product product = _productRepository.Insert(new Product("TShirt"));

                int count = _productRepository.Count();

                uow.Complete();
            }
        }
    }
}
