using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Stove.Domain.Entities.Auditing;

namespace Stove.Tests.SampleApplication.Domain.Entities
{
    [Table(nameof(Message))]
    public class Message : FullAuditedEntity
    {
        protected Message()
        {
        }

        public Message(string subject) : this()
        {
            Subject = subject;
        }

        [Required]
        public string Subject { get; set; }
    }
}
