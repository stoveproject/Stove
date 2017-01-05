using System;
using System.Collections.Generic;
using System.Linq;

using Autofac.Extras.IocManager;

using Stove.Bootstrapping;
using Stove.Events.Bus;
using Stove.Events.Bus.Factories;
using Stove.Events.Bus.Handlers;

namespace Stove.Events
{
    public class EventBusBootstrapper : StoveBootstrapper
    {
        private readonly IEventBus _eventBus;
        private readonly IResolver _resolver;

        public EventBusBootstrapper(IResolver resolver, IEventBus eventBus)
        {
            _resolver = resolver;
            _eventBus = eventBus;
        }

        public override void Start()
        {
            List<Type> serviceTypes = _resolver.GetRegisteredServices().ToList();

            if (!serviceTypes.Any(x => typeof(IEventHandler).IsAssignableFrom(x)))
            {
                return;
            }

            IEnumerable<Type> interfaces = serviceTypes.SelectMany(x => x.GetInterfaces());

            foreach (Type @interface in interfaces)
            {
                if (!typeof(IEventHandler).IsAssignableFrom(@interface))
                {
                    continue;
                }

                Type impl = serviceTypes.ToList().FirstOrDefault(x => @interface.IsAssignableFrom(x));

                Type[] genericArgs = @interface.GetGenericArguments();
                if (genericArgs.Length == 1)
                {
                    _eventBus.Register(genericArgs[0], new IocHandlerFactory(_resolver.Resolve<IScopeResolver>(), impl));
                }
            }
        }
    }
}
