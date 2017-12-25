using System;
using System.Collections.Generic;

using Stove.Domain.Entities;
using Stove.Domain.Entities.Auditing;

namespace Stove.EntityFrameworkCore.Tests.Domain
{
    public class Blog : AggregateRoot, IHasCreationTime
    {
        public Blog()
        {
            Register<BlogUrlChangedEvent>(When);
        }

        public Blog(string name, string url) : this()
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            Name = name;
            Url = url;
        }

        public string Name { get; set; }

        public string Url { get; protected set; }

        public ICollection<Post> Posts { get; set; }

        public DateTime CreationTime { get; set; }

        private void When(BlogUrlChangedEvent @event)
        {
            Url = @event.Url;
        }

        public void ChangeUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            ApplyChange(new BlogUrlChangedEvent(this, url));
        }
    }
}
