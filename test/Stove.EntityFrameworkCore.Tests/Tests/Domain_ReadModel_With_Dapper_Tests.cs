using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Shouldly;

using Stove.Dapper.Repositories;
using Stove.Domain.Uow;
using Stove.EntityFrameworkCore.Tests.Domain_Product;
using Stove.EntityFrameworkCore.Tests.Ef;

using Xunit;

namespace Stove.EntityFrameworkCore.Tests.Tests
{
	public class Domain_ReadModel_With_Dapper_Tests : EntityFrameworkCoreTestBase
	{
		private readonly IDapperRepository<Product> _productDapperRepository;

		public Domain_ReadModel_With_Dapper_Tests()
		{
			Building(builder =>
			{
				RegisterInMemoryDbContext<ProductDbContext>(builder, opt => new ProductDbContext(opt));
			}).Ok();

			UsingDbContext<ProductDbContext>(context =>
			{
				context.Products.Add(new Product("Adidas Awesome Sneaker", "adidas_code_1"));
				context.Products.Add(new Product("Adidas Awesome Sweatshirt", "adidas_code_2"));
			});

			_productDapperRepository = The<IDapperRepository<Product>>();
		}

		[Fact]
		public async Task Should_Get_All_Products()
		{
			using (var uow = The<IUnitOfWorkManager>().Begin())
			{
				IEnumerable<Product> products = await _productDapperRepository.QueryAsync<Product>("SELECT * FROM Products ORDER BY Id");

				products.Count().ShouldBe(2);

				products.ElementAt(0).ProductCode.ShouldBe("adidas_code_1");
				products.ElementAt(1).ProductCode.ShouldBe("adidas_code_2");

				await uow.CompleteAsync();
			}
		}

		[Fact]
		public async Task Should_Get_Product_With_Filter()
		{
			using (var uow = The<IUnitOfWorkManager>().Begin())
			{
				IEnumerable<Product> products = await _productDapperRepository.QueryAsync<Product>("SELECT * FROM Products WHERE Title=@Title", new { Title = "Adidas Awesome Sneaker" });

				products.Count().ShouldBe(1);
				products.First().ProductCode.ShouldBe("adidas_code_1");

				await uow.CompleteAsync();
			}
		}
	}
}
