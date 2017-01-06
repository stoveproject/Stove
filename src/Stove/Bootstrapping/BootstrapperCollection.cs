using System;
using System.Collections.Generic;

namespace Stove.Bootstrapping
{
    public class BootstrapperCollection : List<BootstrapperInfo>
    {
        public BootstrapperCollection(Type initialBootstrapperType)
        {
            InitialBootstrapperType = initialBootstrapperType;
        }

        public Type InitialBootstrapperType { get; }
    }
}
