using JetBrains.Annotations;

namespace Stove.Configuration
{
    public class ModuleConfigurations : IModuleConfigurations
    {
        public ModuleConfigurations([NotNull] IStoveStartupConfiguration stoveConfiguration)
        {
            StoveConfiguration = stoveConfiguration;
        }

        public IStoveStartupConfiguration StoveConfiguration { get; }
    }
}
