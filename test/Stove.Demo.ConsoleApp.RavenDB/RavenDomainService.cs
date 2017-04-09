using System.Collections.Generic;

using Stove.Domain.Repositories;
using Stove.Domain.Services;
using Stove.Domain.Uow;
using Stove.Runtime.Session;

namespace Stove.Demo.ConsoleApp.RavenDB
{
    public class RavenDomainService : DomainService
    {
        private readonly IRepository<Product> _productRepository;

        public RavenDomainService(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public IStoveSession Session { get; set; }

        public void DoSomeStuff()
        {
            using (Session.Use(266))
            {
                using (IUnitOfWorkCompleteHandle uow = UnitOfWorkManager.Begin())
                {
                    Product product = _productRepository.Insert(new Product("TShirt"));
                    product.Name = "Kazak";

                    int count = _productRepository.Count();

                    uow.Complete();
                }

                Product updatedProduct;
                using (IUnitOfWorkCompleteHandle uow = UnitOfWorkManager.Begin())
                {
                    updatedProduct = _productRepository.FirstOrDefault(x => x.Name == "Kazak");

                    uow.Complete();
                }

                updatedProduct.Name = "SecondKazak";

                using (IUnitOfWorkCompleteHandle uow = UnitOfWorkManager.Begin())
                {
                    _productRepository.Update(updatedProduct);

                    uow.Complete();
                }

                using (IUnitOfWorkCompleteHandle uow = UnitOfWorkManager.Begin())
                {
                    _productRepository.Delete(updatedProduct);

                    uow.Complete();
                }

                using (IUnitOfWorkCompleteHandle uow = UnitOfWorkManager.Begin())
                {
                    _productRepository.Insert(new Product("ThirdKazak"));
                    _productRepository.Insert(new Product("ForthKazak"));

                    uow.Complete();
                }

                using (IUnitOfWorkCompleteHandle uow = UnitOfWorkManager.Begin())
                {
                    Product product = _productRepository.FirstOrDefault(x => x.Name == "SecondKazak");

                    List<Product> products = _productRepository.GetAllList(x => x.Name == "ThirdKazak" || x.Name == "ForthKazak" || x.Name== "SecondKazak");

                    uow.Complete();
                }
            }
        }
    }
}
