using System.Collections.Generic;

namespace Stove.Bootstrapping
{
    public interface IBootstrapperManager
    {
        IReadOnlyList<BootstrapperInfo> Bootstrappers { get; }

        void StartBootstrappers();
    }
}
