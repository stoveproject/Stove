using System.Linq;

using Shouldly;

using Stove.Domain.Repositories;
using Stove.Domain.Uow;
using Stove.Tests.SampleApplication.Domain.Entities;

using Xunit;

namespace Stove.Tests.SampleApplication.Uow
{
	public class LazyLoading_Tests : SampleApplicationTestBase
	{
		private readonly IRepository<Product> _productRepository;
		private readonly IUnitOfWorkManager _unitOfWorkManager;

		public LazyLoading_Tests()
		{
			Building(builder => { }).Ok();

			_unitOfWorkManager = The<IUnitOfWorkManager>();
			_productRepository = The<IRepository<Product>>();
		}

		[Fact]
		public void lazy_loading_disable_should_work_on_spesific_uow_scope()
		{
			int productId;

			using (IUnitOfWorkCompleteHandle uow = _unitOfWorkManager.Begin())
			{
				var product = new Product("Elma");
				product.AddCategory("Armut");
				productId = _productRepository.InsertAndGetId(product);

				uow.Complete();
			}

			using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin(new UnitOfWorkOptions { IsLazyLoadEnabled = false }))
			{
				Product product = _productRepository.GetAll().First(x => x.Id == productId);

				product.ProductCategories.Count.ShouldBe(0);

				uow.Complete();
			}
		}

		[Fact]
		public void lazy_loading_enable_should_work_on_spesific_uow_scope()
		{
			int productId;

			using (IUnitOfWorkCompleteHandle uow = _unitOfWorkManager.Begin())
			{
				var product = new Product("Elma");
				product.AddCategory("Armut");
				product.AddCategory("Şeftali");
				productId = _productRepository.InsertAndGetId(product);

				uow.Complete();
			}

			using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin())
			{
				Product product = _productRepository.GetAll().First(x => x.Id == productId);

				product.ProductCategories.Count.ShouldBe(2);

				uow.Complete();
			}
		}
	}
}
