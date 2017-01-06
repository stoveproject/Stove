using System;
using System.Collections.Generic;

using Stove.JetBrains.Annotations;

namespace Stove.Bootstrapping
{
    public class BootstrapperInfo
    {
        public BootstrapperInfo([NotNull] Type type)
        {
            Type = type;
            Dependencies = new List<BootstrapperInfo>();
        }

        /// <summary>
        ///     Type of the module.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        ///     All dependent modules of this module.
        /// </summary>
        public List<BootstrapperInfo> Dependencies { get; }
    }
}
