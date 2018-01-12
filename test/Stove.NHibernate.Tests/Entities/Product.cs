using Stove.Domain.Entities.Auditing;
using Stove.NHibernate.Tests.Entities.Events;

namespace Stove.NHibernate.Tests.Entities
{
    public class Product : FullAuditedAggregateRoot
    {
        protected Product()
        {
            Register<ProductNameFixed>(When);
            Register<ProductDeletedEvent>(When);
            Register<ProductCreatedEvent>(When);
        }

        public Product(string name) : this()
        {
            Name = name;

            ApplyChange(
                new ProductCreatedEvent(name)
            );
        }

        public virtual string Name { get; set; }

        private void When(ProductCreatedEvent @event)
        {
            Name = @event.Name;
        }

        private void When(ProductDeletedEvent @event)
        {
            IsDeleted = true;
        }

        private void When(ProductNameFixed @event)
        {
            Name = @event.Name;
        }

        public virtual void FixName(string name)
        {
            ApplyChange(
                new ProductNameFixed(name)
            );
        }

        public virtual void Delete()
        {
            ApplyChange(
                new ProductDeletedEvent(Id, Name)
            );
        }
    }
}
