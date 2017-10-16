using Stove.Domain.Entities;

namespace Stove.Couchbase.Tests.Entities
{
    public class Product : Entity<string>
    {
        protected Product()
        {
        }

        public Product(string name) : this()
        {
            Name = name;
        }

        public virtual string Name { get; set; }
    }
}
