using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Stove.Domain.Entities.Auditing;

namespace Stove.Demo.ConsoleApp.Entities
{
    [Table("Mails")]
    public class Mail : FullAuditedEntity<Guid>
    {
        protected Mail()
        {
        }

        public Mail(string subject) : this()
        {
            Subject = subject;
        }

        [Required]
        public string Subject { get; set; }
    }
}
