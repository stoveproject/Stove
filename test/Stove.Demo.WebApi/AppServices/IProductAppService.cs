using Stove.Demo.WebApi.Dtos;

namespace Stove.Demo.WebApi.AppServices
{
    public interface IProductAppService
    {
        AddProductOutput AddProduct(AddProductInput input);
    }
}
