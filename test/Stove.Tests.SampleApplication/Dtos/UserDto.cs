using System;

namespace Stove.Tests.SampleApplication.Dtos
{
    public class UserDto
    {
        public virtual string Name { get; set; }

        public virtual string Surname { get; set; }

        public virtual string Email { get; set; }

        public virtual DateTime CreationTime { get; set; }
    }
}
