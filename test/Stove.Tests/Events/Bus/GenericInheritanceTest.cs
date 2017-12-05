using Shouldly;

using Stove.Domain.Entities;
using Stove.Events.Bus.Entities;

using Xunit;

namespace Stove.Tests.Events.Bus
{
    public class GenericInheritanceTest : EventBusTestBase
    {
        [Fact]
        public void Should_Trigger_For_Inherited_Generic_1()
        {
            var triggeredEvent = false;

            EventBus.Register<EntityChangedEventData<Person>>(
                eventData =>
                {
                    eventData.Entity.Id.ShouldBe(42);
                    triggeredEvent = true;
                });

            EventBus.Publish(new EntityUpdatedEventData<Person>(new Person { Id = 42 }));

            triggeredEvent.ShouldBe(true);
        }

        [Fact]
        public void Should_Trigger_For_Inherited_Generic_2()
        {
            var triggeredEvent = false;

            EventBus.Register<EntityChangedEventData<Person>>(
                eventData =>
                {
                    eventData.Entity.Id.ShouldBe(42);
                    triggeredEvent = true;
                });

            EventBus.Publish(new EntityChangedEventData<Student>(new Student { Id = 42 }));

            triggeredEvent.ShouldBe(true);
        }
        
        
        public class Person : Entity
        {
            
        }

        public class Student : Person
        {
            
        }
    }
}