using Stove.Bootstrapping;
using Stove.Bootstrapping.Bootstrappers;
using Stove.EntityFramework.Bootstrappers;

namespace Stove.Tests.SampleApplication
{
    [DependsOn(
        typeof(StoveKernelBootstrapper),
        typeof(StoveEntityFrameworkBootstrapper)
    )]
    public class SampleApplicationBootstrapper : StoveBootstrapper
    {
    }
}
