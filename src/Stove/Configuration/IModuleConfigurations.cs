using JetBrains.Annotations;

namespace Stove.Configuration
{
    public interface IModuleConfigurations
    {
        [NotNull]
        IStoveStartupConfiguration StoveConfiguration { get; }
    }
}
