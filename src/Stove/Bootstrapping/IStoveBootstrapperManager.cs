using System;
using System.Collections.Generic;

namespace Stove.Bootstrapping
{
    public interface IStoveBootstrapperManager
    {
        IReadOnlyList<BootstrapperInfo> Bootstrappers { get; }

        void StartBootstrappers(Type starterBootStrapperType);
    }
}
