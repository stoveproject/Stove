using System.Collections.Generic;
using System.Reflection;

namespace Stove.Reflection
{
    /// <summary>
    ///     This interface is used to get assemblies in the application.
    ///     It may not return all assemblies, but those are related with modules.
    /// </summary>
    public interface IStoveAssemblyFinder
    {
        /// <summary>
        ///     Gets all assemblies.
        /// </summary>
        /// <returns>List of assemblies</returns>
        List<Assembly> GetAllAssemblies();
    }
}
