using System.Web.Http;

using Stove.Demo.WebApi.AppServices;
using Stove.Demo.WebApi.Dtos;

namespace Stove.Demo.WebApi.Controllers
{
    [RoutePrefix("product")]
    public class ProductController : ApiController
    {
        private readonly IProductAppService _productAppService;

        public ProductController(IProductAppService productAppService)
        {
            _productAppService = productAppService;
        }

        [Route("Add")]
        public AddProductOutput AddProduct(AddProductInput input)
        {
            return _productAppService.AddProduct(input);
        }
    }
}
