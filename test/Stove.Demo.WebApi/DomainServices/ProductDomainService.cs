using Stove.Demo.WebApi.Entities;
using Stove.Domain.Repositories;
using Stove.Domain.Services;

namespace Stove.Demo.WebApi.DomainServices
{
    public class ProductDomainService : DomainService
    {
        private readonly IRepository<Product> _productRepository;

        public ProductDomainService(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public Product Add(string productName)
        {
            var product = new Product(productName);

            _productRepository.InsertAndGetId(product);

            return product;
        }
    }
}
