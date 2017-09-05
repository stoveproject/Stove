using Stove.Bootstrapping;
using Stove.Mapster;

namespace Stove.Tests.SampleApplication
{
    [DependsOn(
        typeof(StoveKernelBootstrapper),
        typeof(StoveEntityFrameworkBootstrapper),
        typeof(StoveMapsterBootstrapper)
    )]
    public class SampleApplicationBootstrapper : StoveBootstrapper
    {
        public override void Start()
        {
            StoveConfiguration.Modules.StoveMapster().Configurators.Add(config =>
            {
                //add mapping....
            });
        }
    }
}
