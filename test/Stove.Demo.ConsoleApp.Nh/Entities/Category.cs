using Stove.Domain.Entities.Auditing;

namespace Stove.Demo.ConsoleApp.Nh.Entities
{
    public class Category : FullAuditedEntity
    {
        protected Category()
        {
        }

        public Category(string name) : this()
        {
            Name = name;
        }

        public virtual string Name { get; set; }
    }
}
