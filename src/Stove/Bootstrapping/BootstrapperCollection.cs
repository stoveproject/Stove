using System;
using System.Collections.Generic;

using Stove.Collections.Extensions;

namespace Stove.Bootstrapping
{
    public class BootstrapperCollection : List<BootstrapperInfo>
    {
        public BootstrapperCollection(Type startupBootstrapperType)
        {
            StartupBootstrapperType = startupBootstrapperType;
        }

        public Type StartupBootstrapperType { get; }

        public static void EnsureKernelBootstrapperToBeFirst(List<BootstrapperInfo> bootstrappers)
        {
            int kernelBootstrapperIndex = bootstrappers.FindIndex(m => m.Type == typeof(StoveKernelBootstrapper));
            if (kernelBootstrapperIndex <= 0)
            {
                //It's already the first!
                return;
            }

            BootstrapperInfo kernelBootstrapper = bootstrappers[kernelBootstrapperIndex];
            bootstrappers.RemoveAt(kernelBootstrapperIndex);
            bootstrappers.Insert(0, kernelBootstrapper);
        }

        public static void EnsureStartupBootstrapperToBeLast(List<BootstrapperInfo> modules, Type startupModuleType)
        {
            int startupModuleIndex = modules.FindIndex(m => m.Type == startupModuleType);
            if (startupModuleIndex >= modules.Count - 1)
            {
                //It's already the last!
                return;
            }

            BootstrapperInfo startupModule = modules[startupModuleIndex];
            modules.RemoveAt(startupModuleIndex);
            modules.Add(startupModule);
        }

        public void EnsureStartupBootstrapperToBeLast()
        {
            EnsureStartupBootstrapperToBeLast(this, StartupBootstrapperType);
        }

        public List<BootstrapperInfo> GetSortedBootstrapperListByDependency()
        {
            List<BootstrapperInfo> sortedBootstrappers = this.SortByDependencies(x => x.Dependencies);
            EnsureKernelBootstrapperToBeFirst(sortedBootstrappers);
            return sortedBootstrappers;
        }

        public void EnsureKernelBootstrapperToBeFirst()
        {
            EnsureKernelBootstrapperToBeFirst(this);
        }
    }
}
