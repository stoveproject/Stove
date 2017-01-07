namespace Stove.Configuration
{
    public class ModuleConfigurations : IModuleConfigurations
    {
        public ModuleConfigurations(IStoveStartupConfiguration stoveConfiguration)
        {
            StoveConfiguration = stoveConfiguration;
        }

        public IStoveStartupConfiguration StoveConfiguration { get; }
    }
}
