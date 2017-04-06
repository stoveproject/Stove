using Stove.Domain.Entities;

namespace Stove.Demo.ConsoleApp.RavenDB
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
