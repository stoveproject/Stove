using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Stove.BackgroundJobs;
using Stove.Domain.Uow;
using Stove.Events.Bus;
using Stove.Events.Bus.Factories;
using Stove.Events.Bus.Handlers;
using Stove.Threading;
using Stove.Threading.BackgrodunWorkers;

namespace Stove.Bootstrapping
{
    public class StoveKernelBootstrapper : StoveBootstrapper
    {
        private readonly IBackgroundWorkerManager _backgroundWorkerManager;
        private readonly IEventBus _eventBus;

        public StoveKernelBootstrapper(IBackgroundWorkerManager backgroundWorkerManager, IEventBus eventBus)
        {
            _backgroundWorkerManager = backgroundWorkerManager;
            _eventBus = eventBus;
        }

        public override void PreStart()
        {
            StoveConfiguration.UnitOfWork.RegisterFilter(StoveDataFilters.SoftDelete, true);
        }

        public override void Start()
        {
            ConfigureEventBus();
            ConfigureBackgroundJobs();
        }

        public override void Shutdown()
        {
            if (StoveConfiguration.BackgroundJobs.IsJobExecutionEnabled)
            {
                _backgroundWorkerManager.StopAndWaitToStop();
            }
        }

        private void ConfigureBackgroundJobs()
        {
            StoveConfiguration.GetConfigurerIfExists<IBackgroundJobConfiguration>().Invoke(StoveConfiguration.BackgroundJobs);

            if (StoveConfiguration.BackgroundJobs.IsJobExecutionEnabled)
            {
                _backgroundWorkerManager.Start();
            }
        }

        private void ConfigureEventBus()
        {
            List<Type> serviceTypes = Resolver.GetRegisteredServices().ToList();

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

                List<Type> handlers = serviceTypes.Where(x => @interface.IsAssignableFrom(x)).ToList();

                Type[] genericArgs = @interface.GetTypeInfo().GetGenericArguments();
                if (genericArgs.Length == 1)
                {
                    handlers.ForEach(handler =>
                        _eventBus.Register(genericArgs[0], new IocHandlerFactory(Resolver, handler)
                        )
                    );
                }
            }
        }
    }
}
