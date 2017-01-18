using System.Collections.Generic;

using Stove.Collections.Extensions;

namespace Stove.Bootstrapping
{
    public class BootstrapperCollection : List<BootstrapperInfo>
    {
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
