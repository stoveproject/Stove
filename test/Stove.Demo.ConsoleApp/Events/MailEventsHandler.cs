using System;

using Autofac.Extras.IocManager;

using Stove.Demo.ConsoleApp.Entities;
using Stove.Events.Bus.Entities;
using Stove.Events.Bus.Handlers;

namespace Stove.Demo.ConsoleApp.Events
{
    public class MailEventsHandler : IEventHandler<EntityCreatedEventData<Mail>>, IEventHandler<EntityUpdatedEventData<Mail>>, ITransientDependency
    {
        public void Handle(EntityCreatedEventData<Mail> eventData)
        {
            Mail mail = eventData.Entity;
            Console.WriteLine($"Mail Entity Created Event, with Subject : {mail.Subject}");
        }

        public void Handle(EntityUpdatedEventData<Mail> eventData)
        {
            Mail mail = eventData.Entity;
            Console.WriteLine($"Mail Entity Updated Event, with Subject : {mail.Subject}");
        }
    }
}
