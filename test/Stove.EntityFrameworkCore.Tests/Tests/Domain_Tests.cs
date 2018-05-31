using Stove.Domain.Repositories;
using Stove.Domain.Uow;
using Stove.EntityFrameworkCore.Tests.Domain_Product;
using Stove.EntityFrameworkCore.Tests.Ef;
using System.Threading.Tasks;
using Xunit;
using Shouldly;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Stove.EntityFrameworkCore.Tests.Tests
{
    public class Domain_Tests : EntityFrameworkCoreTestBase
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<ProductVariant> _productVariantRepository;

        public Domain_Tests()
        {
            Building(builder =>
            {
                RegisterInMemoryDbContext<ProductDbContext>(builder, opt => new ProductDbContext(opt));
            }).Ok();

            UsingDbContext<ProductDbContext>(context =>
            {
                Product adidasAwesomeSneaker = context.Products.Add(new Product("Adidas Awesome Sneaker", "adidas_code_1")).Entity;
                ProductVariant adidasSneakerXL = context.ProductVariants.Add(new ProductVariant(adidasAwesomeSneaker, "adidas_barcode_1", "Number")).Entity;

                context.VaraintValues.Add(new VariantValue(adidasSneakerXL, "40"));
                context.VaraintValues.Add(new VariantValue(adidasSneakerXL, "42"));
                context.VaraintValues.Add(new VariantValue(adidasSneakerXL, "43"));
            });

            _productRepository = The<IRepository<Product>>();
            _productVariantRepository = The<IRepository<ProductVariant>>();
        }

        [Fact]
        public async Task Add_Variant_to_Product()
        {
            using (var uow = The<IUnitOfWorkManager>().Begin())
            {
                Product product = await _productRepository.FirstOrDefaultAsync(x => x.Title == "Adidas Awesome Sneaker");
                product.AddVariant("barcode_1", "43");

                await uow.CompleteAsync();
            }
                        
            await UsingDbContextAsync<ProductDbContext>(async context =>
            {
                ProductVariant productVariant = await context.ProductVariants.FirstOrDefaultAsync(v => v.Barcode == "barcode_1");
                productVariant.ShouldNotBeNull();
                productVariant.Product.Title.ShouldBe("Adidas Awesome Sneaker");
            });
        }

        [Fact]
        public async Task Add_Value_to_Variant()
        {
            using (var uow = The<IUnitOfWorkManager>().Begin())
            {
                Product product = await _productRepository.FirstOrDefaultAsync(x => x.Title == "Adidas Awesome Sneaker");
                ProductVariant variant = await _productVariantRepository.FirstOrDefaultAsync(v => v.Barcode == "adidas_barcode_1");

                product.AddVariantValue(variant, "46");

                await uow.CompleteAsync();
            }

            await UsingDbContextAsync<ProductDbContext>(async context =>
            {
                ProductVariant productVariant = await context.ProductVariants.FirstOrDefaultAsync(v => v.Barcode == "adidas_barcode_1");
                productVariant.Values.Count.ShouldBe(4);
                productVariant.Values.Any(sv => sv.Value == "46").ShouldBe(true);
            });
        }

    }
}
