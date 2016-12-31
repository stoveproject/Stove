using Stove.Domain.Entities.Auditing;

namespace Stove.Z.Demo
{
    public class Person : FullAuditedEntity
    {
        public virtual string Name { get; set; }
    }
}
