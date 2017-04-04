using Stove.Domain.Entities;

namespace Stove.RavenDB.Tests.Entities
{
    public class Product : Entity
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
