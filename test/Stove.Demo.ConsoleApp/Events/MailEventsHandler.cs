using System;

using Autofac.Extras.IocManager;

using Stove.Demo.ConsoleApp.Entities;
using Stove.Events.Bus.Entities;
using Stove.Events.Bus.Handlers;

namespace Stove.Demo.ConsoleApp.Events
{
    public class MailEventsHandler : IEventHandler<EntityCreatedEvent<Mail>>, IEventHandler<EntityUpdatedEvent<Mail>>, ITransientDependency
    {
        public void Handle(EntityCreatedEvent<Mail> @event)
        {
            Mail mail = @event.Entity;
            Console.WriteLine($"Mail Entity Created Event, with Subject : {mail.Subject}");
        }

        public void Handle(EntityUpdatedEvent<Mail> @event)
        {
            Mail mail = @event.Entity;
            Console.WriteLine($"Mail Entity Updated Event, with Subject : {mail.Subject}");
        }
    }
}
