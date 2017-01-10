using System.ComponentModel.DataAnnotations.Schema;

using Stove.Domain.Entities.Auditing;

namespace Stove.Tests.SampleApplication.Domain.Entities
{
    [Table(nameof(Product))]
    public class Product : CreationAuditedEntity<long, User>
    {
        public virtual string Name { get; set; }

        public virtual string Code { get; set; }

        public virtual string Description { get; set; }
    }
}
