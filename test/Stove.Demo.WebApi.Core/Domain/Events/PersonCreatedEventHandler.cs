using System;

using Autofac.Extras.IocManager;

using Stove.Demo.WebApi.Core.Domain.Entities;
using Stove.Events.Bus.Entities;
using Stove.Events.Bus.Handlers;

namespace Stove.Demo.WebApi.Core.Domain.Events
{
    public class PersonCreatedEventHandler : IEventHandler<EntityCreatedEventData<Person>>, ITransientDependency
    {
        public void Handle(EntityCreatedEventData<Person> eventData)
        {
            Person person = eventData.Entity;
            Console.WriteLine($"Person Entity Created Event, with Subject : {person.Name}");
        }
    }
}
