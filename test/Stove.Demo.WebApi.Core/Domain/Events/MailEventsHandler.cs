using System;

using Autofac.Extras.IocManager;

using Stove.Demo.WebApi.Core.Domain.Entities;
using Stove.Events.Bus.Entities;
using Stove.Events.Bus.Handlers;

namespace Stove.Demo.WebApi.Core.Domain.Events
{
    public class MailEventsHandler : IEventHandler<EntityCreatedEventData<Mail>>, IEventHandler<EntityUpdatedEventData<Mail>>, ITransientDependency
    {
        public void HandleEvent(EntityCreatedEventData<Mail> eventData)
        {
            Mail mail = eventData.Entity;
            Console.WriteLine($"Mail Entity Created Event, with Subject : {mail.Subject}");
        }

        public void HandleEvent(EntityUpdatedEventData<Mail> eventData)
        {
            Mail mail = eventData.Entity;
            Console.WriteLine($"Mail Entity Updated Event, with Subject : {mail.Subject}");
        }
    }
}
