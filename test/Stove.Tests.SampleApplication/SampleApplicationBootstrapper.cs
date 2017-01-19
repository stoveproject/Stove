using Stove.Bootstrapping;
using Stove.EntityFramework;
using Stove.Mapster.Bootstrappers;
using Stove.Mapster.Mapster;

namespace Stove.Tests.SampleApplication
{
    [StarterBootstrapper]
    [DependsOn(
        typeof(StoveKernelBootstrapper),
        typeof(StoveEntityFrameworkBootstrapper),
        typeof(StoveMapsterBootstrapper)
    )]
    public class SampleApplicationBootstrapper : StoveBootstrapper
    {
        public override void Start()
        {
            Configuration.Modules.StoveMapster().Configurators.Add(config =>
            {
                //add mapping....
            });
        }
    }
}
