using Stove.Bootstrapping;
using Stove.EntityFramework;
using Stove.Mapster.Bootstrappers;
using Stove.Mapster.Mapster;
using Stove.Tests.SampleApplication.Domain.Entities;

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
            Configuration.Modules.StoveMapster().Configurators.Add(config =>
            {
                //add mapping....
            });
        }
    }
}
