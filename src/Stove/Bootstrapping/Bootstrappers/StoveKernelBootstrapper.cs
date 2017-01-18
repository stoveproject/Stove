using System;
using System.Collections.Generic;
using System.Linq;

using Autofac.Extras.IocManager;

using Stove.BackgroundJobs;
using Stove.Domain.Uow;
using Stove.Events.Bus;
using Stove.Events.Bus.Factories;
using Stove.Events.Bus.Handlers;
using Stove.Threading.BackgrodunWorkers;

namespace Stove.Bootstrapping.Bootstrappers
{
    public class StoveKernelBootstrapper : StoveBootstrapper
    {
        private readonly Func<IBackgroundJobConfiguration, IBackgroundJobConfiguration> _backgroundJobConfigurer;
        private readonly IBackgroundWorkerManager _backgroundWorkerManager;
        private readonly IEventBus _eventBus;

        public StoveKernelBootstrapper(
            Func<IBackgroundJobConfiguration, IBackgroundJobConfiguration> backgroundJobConfigurer,
            IBackgroundWorkerManager backgroundWorkerManager,
            IEventBus eventBus)
        {
            _backgroundJobConfigurer = backgroundJobConfigurer;
            _backgroundWorkerManager = backgroundWorkerManager;
            _eventBus = eventBus;
        }

        public override void PreStart()
        {
            Configuration.UnitOfWork.RegisterFilter(StoveDataFilters.SoftDelete, true);
        }

        public override void Start()
        {
            ConfigureEventBus();
            ConfigureBackgroundJobs();
        }

        private void ConfigureBackgroundJobs()
        {
            _backgroundJobConfigurer(Configuration.BackgroundJobs);

            if (Configuration.BackgroundJobs.IsJobExecutionEnabled)
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

                Type impl = serviceTypes.ToList().FirstOrDefault(x => @interface.IsAssignableFrom(x));

                Type[] genericArgs = @interface.GetGenericArguments();
                if (genericArgs.Length == 1)
                {
                    _eventBus.Register(genericArgs[0], new IocHandlerFactory(Resolver.Resolve<IScopeResolver>(), impl));
                }
            }
        }
    }
}
