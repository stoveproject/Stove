using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Stove.Domain.Entities;
using Stove.Domain.Entities.Auditing;
using Stove.Events.Bus;
using Stove.Mapster;
using Stove.Tests.SampleApplication.Dtos;

namespace Stove.Tests.SampleApplication.Domain.Entities
{
    [Table(nameof(User))]
    [AutoMapTo(typeof(UserDto))]
    public class User : AggregateRoot, IHasCreationTime
    {
        [Required]
        public virtual string Name { get; set; }

        [Required]
        public virtual string Surname { get; set; }

        [Required]
        public virtual string Email { get; set; }

        [Required]
        public virtual DateTime CreationTime { get; set; }

        public static User Create(string name, string surname, string email)
        {
            var user = new User();
            user.Name = name;
            user.Surname = surname;
            user.Email = email;

            user.ApplyChange(
                new UserCreatedEvent(name)
            );

            return user;
        }
    }

    public class UserCreatedEvent : Event
    {
        public readonly string Name;

        public UserCreatedEvent(string name)
        {
            Name = name;
        }
    }
}
