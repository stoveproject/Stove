using System;

namespace Stove.Bootstrapping
{
    public class BootstrapperManager : IBootstrapperManager
    {
        private BootstrapperCollection _bootstrappers;

        public BootstrapperManager(Type initialBootstrapperType)
        {
            _bootstrappers = new BootstrapperCollection(initialBootstrapperType);
        }

        public BootstrapperInfo InitialBootstrapper { get; private set; }
    }
}
