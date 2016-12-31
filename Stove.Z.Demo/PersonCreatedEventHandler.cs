using Autofac.Extras.IocManager;

using Stove.Events.Bus.Entities;
using Stove.Events.Bus.Handlers;

namespace Stove.Demo
{
    public class PersonCreatedEventHandler : IEventHandler<EntityCreatedEventData<Person>>, ITransientDependency
    {
        public void HandleEvent(EntityCreatedEventData<Person> eventData)
        {
            Person a = eventData.Entity;
        }
    }
}
