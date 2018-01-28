using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Stove.Domain.Entities;
using Stove.Events.Bus;

namespace Stove.Tests.SampleApplication.Domain.Entities
{
    public class ProductBundle : AggregateRoot<Guid>
    {
        public static readonly Func<ProductBundle> Factory = () => new ProductBundle();

        protected ProductBundle()
        {
            Register<ProductBundleCreated>(When);
            Register<ProductAddedToBundle>(When);
        }

        public ICollection<int> ProductIds
        {
            get { return _productIds.Split(',').Select(int.Parse).ToList(); }
        }

        private string _productIds;

        public static Expression<Func<ProductBundle, string>> ProductIdsExpression = bundle => bundle._productIds;

        public string Name { get; protected set; }

        private void When(ProductAddedToBundle @event)
        {
            _productIds = _productIds + $",{@event.ProductId}";
        }

        private void When(ProductBundleCreated @event)
        {
            Name = @event.Name;
            Id = @event.Id;
        }

        public static ProductBundle Create(Guid id, string name)
        {
            ProductBundle aggregate = Factory();

            aggregate.ApplyChange(
                new ProductBundleCreated(id, name)
            );

            return aggregate;
        }
    }

    public class ProductBundleCreated : Event
    {
        public readonly Guid Id;
        public readonly string Name;

        public ProductBundleCreated(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    public class ProductAddedToBundle : Event
    {
        public readonly int Id;
        public readonly int ProductId;

        public ProductAddedToBundle(int id, int productId)
        {
            Id = id;
            ProductId = productId;
        }
    }
}
