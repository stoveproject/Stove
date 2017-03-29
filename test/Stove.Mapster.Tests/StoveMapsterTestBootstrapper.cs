using Stove.Bootstrapping;

namespace Stove.Mapster.Tests
{
    [DependsOn(
        typeof(StoveMapsterBootstrapper)
    )]
    public class StoveMapsterTestBootstrapper : StoveBootstrapper
    {
        public override void PreStart()
        {
            Configuration.Modules.StoveMapster().Configurators.Add(config =>
            {
                config.RequireExplicitMapping = true;
            });
        }
    }
}
