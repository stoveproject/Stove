using System.ComponentModel.DataAnnotations.Schema;

using Stove.Domain.Entities.Auditing;

namespace Stove.Tests.SampleApplication.Domain.Entities
{
    [Table("Product")]
    public class Product : CreationAuditedEntity<long, User>
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }
    }
}
