using Autofac.Extras.IocManager;

using Stove.Demo.Entities;
using Stove.Events.Bus.Entities;
using Stove.Events.Bus.Handlers;

namespace Stove.Demo.Events
{
    public class PersonCreatedEventHandler : IEventHandler<EntityCreatedEventData<Person>>, ITransientDependency
    {
        public void HandleEvent(EntityCreatedEventData<Person> eventData)
        {
            Person a = eventData.Entity;
        }
    }
}
