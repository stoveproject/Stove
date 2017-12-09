using System.Collections.Generic;
using System.Linq;

using Shouldly;

using Stove.Linq.Extensions;

using Xunit;

namespace Stove.Tests.Linq.Extensions
{
    public class QueryableExtensionsTests
    {
        [Fact]
        public void PageBy_with_skip_and_maxresultcount_should_work()
        {
            var products = new List<Product>
            {
                new Product { Name = "Oguzhan" },
                new Product { Name = "Soykan" }
            };

            IQueryable<Product> productsQueryable = products.AsQueryable();

            IQueryable<Product> pagedQueryable = productsQueryable.PageBy(0, 2);

            pagedQueryable.ToList().Count.ShouldBe(2);
        }

        [Fact]
        public void WhereIf_should_work()
        {
            var products = new List<Product>
            {
                new Product { Name = "Oguzhan" },
                new Product { Name = "Soykan" }
            };

            IQueryable<Product> productsQueryable = products.AsQueryable();

            IQueryable<Product> pagedQueryable = productsQueryable.WhereIf(products.Count == 2, (product, i) => product.Name == "Oguzhan");

            pagedQueryable.ToList().Count.ShouldBe(1);
        }

        [Fact]
        public void WhereIf_should_wor2k()
        {
            var products = new List<Product>
            {
                new Product { Name = "Oguzhan" },
                new Product { Name = "Soykan" }
            };

            IQueryable<Product> productsQueryable = products.AsQueryable();

            IQueryable<Product> pagedQueryable = productsQueryable.WhereIf(products.Count == 2, product => product.Name == "Oguzhan");

            pagedQueryable.ToList().Count.ShouldBe(1);
        }

        private class Product
        {
            public string Name { get; set; }
        }
    }
}
