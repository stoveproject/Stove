using Stove.Domain.Entities;
using Stove.Events.Bus.Entities;

using Xunit;

namespace Stove.Tests.Experimental
{
    public class GenericMethodCallTests
    {
        [Fact]
        public void Test_Method_BaseEvent_BaseArg()
        {
            Method_BaseEvent_BaseArg(new EntityEvent<Person>(new Person())); //TODO: <Student>
            Method_BaseEvent_BaseArg(new EntityEvent<Person>(new Student())); //TODO: <Student>
            Method_BaseEvent_BaseArg(new EntityUpdatedEvent<Person>(new Person())); //TODO: <Student>
            Method_BaseEvent_BaseArg(new EntityUpdatedEvent<Person>(new Student())); //TODO: <Student>
        }

        public void Method_BaseEvent_BaseArg(EntityEvent<Person> data)
        {
        }

        [Fact]
        public void Test_Method_BaseEvent_DerivedArg()
        {
            Method_BaseEvent_DerivedArg(new EntityEvent<Student>(new Student()));
            Method_BaseEvent_DerivedArg(new EntityUpdatedEvent<Student>(new Student()));
        }

        public void Method_BaseEvent_DerivedArg(EntityEvent<Student> data)
        {
        }

        [Fact]
        public void Test_Method_DerivedEvent_BaseArg()
        {
            Method_DerivedEvent_BaseArg(new EntityUpdatedEvent<Person>(new Person()));
            Method_DerivedEvent_BaseArg(new EntityUpdatedEvent<Person>(new Student()));
        }

        public void Method_DerivedEvent_BaseArg(EntityUpdatedEvent<Person> data)
        {
        }

        [Fact]
        public void Test_Method_DerivedEvent_DerivedArg()
        {
            Method_DerivedEvent_DerivedArg(new EntityUpdatedEvent<Student>(new Student()));
        }

        public void Method_DerivedEvent_DerivedArg(EntityUpdatedEvent<Student> data)
        {
        }

        public class Person : Entity
        {
        }

        public class Student : Person
        {
        }
    }
}
