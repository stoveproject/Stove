using Raven.Client.Linq;

using Stove.Domain.Repositories;
using Stove.Domain.Services;
using Stove.Domain.Uow;
using Stove.Extensions;

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

                var a = _productRepository.GetAll().As<IRavenQueryable<Product>>().Customize(x => x.NoTracking());

                int count = _productRepository.Count();

                uow.Complete();
            }
        }
    }
}
