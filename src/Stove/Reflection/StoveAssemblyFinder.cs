using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Stove.Bootstrapping;

namespace Stove.Reflection
{
    public class StoveAssemblyFinder : IStoveAssemblyFinder
    {
        private readonly IStoveBootstrapperManager _stoveBootstrapperManager;

        public StoveAssemblyFinder(IStoveBootstrapperManager stoveBootstrapperManager)
        {
            _stoveBootstrapperManager = stoveBootstrapperManager;
        }

        public List<Assembly> GetAllAssemblies()
        {
            var assemblies = new List<Assembly>();

            foreach (BootstrapperInfo bootstrapperInfo in _stoveBootstrapperManager.Bootstrappers)
            {
                assemblies.Add(bootstrapperInfo.Assembly);
                assemblies.AddRange(bootstrapperInfo.Instance.GetAdditionalAssemblies());
            }

            return assemblies.Distinct().ToList();
        }
    }
}
