using Stove.Application.Services;
using Stove.Demo.WebApi.DomainServices;
using Stove.Demo.WebApi.Dtos;
using Stove.Demo.WebApi.Entities;

namespace Stove.Demo.WebApi.AppServices
{
    public class ProductAppService : ApplicationService, IProductAppService
    {
        private readonly ProductDomainService _productDomainService;

        public ProductAppService(ProductDomainService productDomainService)
        {
            _productDomainService = productDomainService;
        }

        public AddProductOutput AddProduct(AddProductInput input)
        {
            Product product = _productDomainService.Add(input.Name);

            return new AddProductOutput
            {
                ProductId = product.Id,
                ProductName = product.Name
            };
        }
    }
}
