using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace Stove.Bootstrapping
{
    public interface IStoveBootstrapperManager
    {
        [NotNull]
        IReadOnlyList<BootstrapperInfo> Bootstrappers { get; }

        void StartBootstrappers([NotNull] Type starterBootStrapperType);

        void ShutdownBootstrappers();
    }
}
