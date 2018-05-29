using System;
using System.ComponentModel.DataAnnotations;

using Stove.Domain.Entities.Auditing;

namespace Stove.EntityFrameworkCore.Tests.Domain
{
    public class Post : AuditedEntity<Guid>
    {
        [Required]
        public virtual Blog Blog { get; set; }

        public virtual string Title { get; set; }

        public virtual string Body { get; set; }

        public Post()
        {
            
        }

        public Post(Blog blog, string title, string body)
        {
            Blog = blog;
            Title = title;
            Body = body;
        }
    }
}