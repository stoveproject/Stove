using System;

using Autofac.Extras.IocManager;

using Stove.Demo.WebApi.Core.Domain.Entities;
using Stove.Events.Bus.Entities;
using Stove.Events.Bus.Handlers;

namespace Stove.Demo.WebApi.Core.Domain.Events
{
    public class PersonCreatedEventHandler : IEventHandler<EntityCreatedEvent<Person>>, ITransientDependency
    {
        public void Handle(EntityCreatedEvent<Person> @event)
        {
            Person person = @event.Entity;
            Console.WriteLine($"Person Entity Created Event, with Subject : {person.Name}");
        }
    }
}
