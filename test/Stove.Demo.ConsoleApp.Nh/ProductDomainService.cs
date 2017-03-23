using Stove.Dapper.Repositories;
using Stove.Demo.ConsoleApp.Nh.Entities;
using Stove.Domain.Repositories;
using Stove.Domain.Services;
using Stove.Domain.Uow;

namespace Stove.Demo.ConsoleApp.Nh
{
    public class ProductDomainService : DomainService
    {
        private readonly IDapperRepository<Product> _productDapperRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public ProductDomainService(
            IDapperRepository<Product> productDapperRepository,
            IRepository<Product> productRepository,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _productDapperRepository = productDapperRepository;
            _productRepository = productRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public void DoSomeCoolStuff()
        {
            using (IUnitOfWorkCompleteHandle uow = _unitOfWorkManager.Begin())
            {
                _productDapperRepository.Insert(new Product("Jean"));

                _productRepository.FirstOrDefault(x => x.Name == "Jean");

                uow.Complete();
            }
        }
    }
}
