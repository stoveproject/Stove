using System;
using System.Linq;

using Autofac;
using Autofac.Extras.IocManager;

using Stove.Reflection.Extensions;

namespace Stove.Bootstrapping
{
    public class StoveMainBootstrapper : IStartable, ITransientDependency
    {
        private readonly IStoveBootstrapperManager _stoveBootstrapperManager;

        public StoveMainBootstrapper(IStoveBootstrapperManager stoveBootstrapperManager)
        {
            _stoveBootstrapperManager = stoveBootstrapperManager;
        }

        public void Start()
        {
            Type starterBootstrapper = typeof(StoveBootstrapper).AssignedTypes()
                                                                .FirstOrDefault(x => x.GetSingleAttributeOrNull<StarterBootstrapper>() != null);

            if (starterBootstrapper == null)
            {
                throw new StoveException("There is no StarterBootstraper, please define a starter bootstrapper in your entry bootstrapper as attribute.");
            }

            _stoveBootstrapperManager.StartBootstrappers(starterBootstrapper);
        }
    }
}
