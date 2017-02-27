using System;
using System.Collections.Generic;
using System.Reflection;

using JetBrains.Annotations;

namespace Stove.Bootstrapping
{
    public class BootstrapperInfo
    {
        public BootstrapperInfo([NotNull] Type type, [NotNull] StoveBootstrapper instance)
        {
            Type = type;
            Assembly = Type.Assembly;
            Instance = instance;
            Dependencies = new List<BootstrapperInfo>();
        }

        [NotNull]
        public StoveBootstrapper Instance { get; }

        [NotNull]
        public Type Type { get; }

        [NotNull]
        public List<BootstrapperInfo> Dependencies { get; }

        [NotNull]
        public Assembly Assembly { get; }
    }
}
