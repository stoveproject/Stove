using Stove.Dapper.Repositories;
using Stove.Demo.ConsoleApp.Nh.Entities;
using Stove.Domain.Repositories;
using Stove.Domain.Services;
using Stove.Domain.Uow;
using Stove.Runtime.Session;

namespace Stove.Demo.ConsoleApp.Nh
{
    public class ProductDomainService : DomainService
    {
        private readonly IDapperRepository<Product> _productDapperRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IDapperRepository<Category> _categoryDapperRepository;

        public ProductDomainService(
            IDapperRepository<Product> productDapperRepository,
            IRepository<Product> productRepository,
            IUnitOfWorkManager unitOfWorkManager,
            IRepository<Category> categoryRepository,
            IDapperRepository<Category> categoryDapperRepository)
        {
            _productDapperRepository = productDapperRepository;
            _productRepository = productRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _categoryRepository = categoryRepository;
            _categoryDapperRepository = categoryDapperRepository;
        }

        public IStoveSession StoveSession { get; set; }

        public void DoSomeCoolStuff()
        {
            using (StoveSession.Use(1))
            {
                using (IUnitOfWorkCompleteHandle uow = _unitOfWorkManager.Begin())
                {
                    _productDapperRepository.Insert(new Product("JeanFromDapper"));

                    Product product = _productRepository.FirstOrDefault(x => x.Name == "JeanFromDapper");

                    _productRepository.InsertAndGetId(new Product("JeanFromNH"));

                    _categoryRepository.Insert(new Category("Selam"));

                    uow.Complete();
                }
            }
        }
    }
}
