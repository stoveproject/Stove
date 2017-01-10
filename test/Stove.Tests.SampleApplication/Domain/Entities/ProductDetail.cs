using System.ComponentModel.DataAnnotations.Schema;

using Stove.Domain.Entities.Auditing;

namespace Stove.Tests.SampleApplication.Domain.Entities
{
    [Table(nameof(ProductDetail))]
    public class ProductDetail : CreationAuditedEntity<long, User>
    {
        public virtual Product Product { get; set; }

        public virtual string Age { get; set; }
    }
}
