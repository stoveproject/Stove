using System;

using Autofac.Extras.IocManager;

using Stove.Demo.ConsoleApp.Entities;
using Stove.Events.Bus.Entities;
using Stove.Events.Bus.Handlers;

namespace Stove.Demo.ConsoleApp.Events
{
    public class PersonCreatedEventHandler : IEventHandler<EntityCreatedEventData<Person>>, ITransientDependency
    {
        public void HandleEvent(EntityCreatedEventData<Person> eventData)
        {
            Person person = eventData.Entity;
            Console.WriteLine($"Person Entity Created Event, with Subject : {person.Name}");
        }
    }
}
