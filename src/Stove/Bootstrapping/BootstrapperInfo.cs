using System;
using System.Collections.Generic;
using System.Reflection;

using JetBrains.Annotations;

namespace Stove.Bootstrapping
{
    public class BootstrapperInfo
    {
        public BootstrapperInfo([NotNull] Type type, StoveBootstrapper instance)
        {
            Type = type;
            Assembly = Type.Assembly;
            Instance = instance;
            Dependencies = new List<BootstrapperInfo>();
        }

        public StoveBootstrapper Instance { get; }

        public Type Type { get; }

        public List<BootstrapperInfo> Dependencies { get; }

        public Assembly Assembly { get; }
    }
}
